using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Google.Cloud.Storage.V1;
using LOLA_SERVER.API.Controllers.Base;
using LOLA_SERVER.API.Models.Images;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace LOLA_SERVER.API.Controllers.Images.v1
{
    [Authorize]
    [Route("api/v1/images")]
    [ApiController]
    public class ImagesController : BaseController
    {

        private readonly StorageClient _storageClient;
        private readonly string _bucketName = "lola-app-e5f71.appspot.com";
        private readonly FirebaseApp _firebaseApp;
        private readonly FirestoreDb _firestoreDb;
        public ImagesController()
        {
            // Inicializar el cliente de Firebase Storage
            string credentialPath = "Utils/Credentials/Firebase-Credentials.json";
            var credential = GoogleCredential.FromFile(credentialPath);
            _storageClient = StorageClient.Create(credential);
            var builder = new FirestoreClientBuilder
            {
                Credential = credential
            };
            _firestoreDb = FirestoreDb.Create("lola-app-e5f71", builder.Build());
        }

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadImage(List< IFormFile> files, [FromQuery] string folder = "")
        {
            if (string.IsNullOrEmpty(folder))
                return ApiResponseError("Es necesario especificar una carpeta.");

            if (files == null || !files.Any())
                return ApiResponseError("No se ha proporcionado un archivo válido.");
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "test";
            try
            {
                var uploadedUrls = new List<string>();

                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        var uniqueFileName = GenerateUniqueFileName(file);
                        var folderSanitazed = SanitazeFolderPath(folder);
                        var filePath = SanitazeFolderPath($"{folderSanitazed}/{uniqueFileName}");

                        using var stream = file.OpenReadStream();
                        var storageObject = await _storageClient.UploadObjectAsync(_bucketName, filePath, file.ContentType, stream, new UploadObjectOptions
                        {
                            PredefinedAcl = PredefinedObjectAcl.PublicRead
                        });

                        var publicUrl = $"https://storage.googleapis.com/{_bucketName}/{storageObject.Name}";
                        uploadedUrls.Add(publicUrl);
                        await SaveImageInfoToFirebase(new ImageInfoDB
                        {
                            FilePath = folderSanitazed,
                            NameHash = uniqueFileName,
                            OriginalFileName = uniqueFileName,
                            CreatedDate = DateTime.UtcNow,
                            UploadDate = DateTime.UtcNow,
                            Url = publicUrl,
                            UserId = userId

                        });
                    }
                }

                return ApiResponse(new { Urls = uploadedUrls });
            }
            catch (Exception ex)
            {
                return ApiResponseServerError($"Error interno al cargar la imagen: {ex.Message}");
            }
        }


        private string SanitazeFolderPath(string PathFolder)
        {

            var sanitized = Regex.Replace(PathFolder, @"[^a-zA-Z0-9/]", "").ToString();

            sanitized = Regex.Replace(sanitized, @"/+", "/");

            return sanitized.Trim().TrimEnd('/').TrimStart('/');

        }


        private string GenerateUniqueFileName(IFormFile file)
        {
            using var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes($"{file.FileName}{DateTime.UtcNow.Ticks}"));
            var fileNameHash = BitConverter.ToString(hash).Replace("-", "").ToLower();

            var fileExtension = Path.GetExtension(file.FileName);
            return $"{fileNameHash}{fileExtension}";

        }


        private async Task SaveImageInfoToFirebase(ImageInfoDB ImageInformation)
        {
            try
            {

                CollectionReference collection = _firestoreDb.Collection("Images");
                await collection.AddAsync(ImageInformation);
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }


        }

    }

}

