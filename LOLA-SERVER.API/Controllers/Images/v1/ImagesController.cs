using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using LOLA_SERVER.API.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace LOLA_SERVER.API.Controllers.Images.v1
{
    //[Authorize]
    [Route("api/v1/images")]
    [ApiController]
    public class ImagesController : BaseController
    {

        private readonly StorageClient _storageClient;
        private readonly string _bucketName = "lola-app-e5f71.appspot.com";

        public ImagesController()
        {
            // Inicializar el cliente de Firebase Storage
            string credentialPath = "Utils/Credentials/Firebase-Credentials.json";
            var credential = GoogleCredential.FromFile(credentialPath);
            _storageClient = StorageClient.Create(credential);
        }

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadImage(List< IFormFile> files, [FromQuery] string folder = "")
        {
            if (string.IsNullOrEmpty(folder))
                return ApiResponseError("Es necesario especificar una carpeta.");

            if (files == null || !files.Any())
                return ApiResponseError("No se ha proporcionado un archivo válido.");

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
                        //await SaveImageInfoToDatabase(publicUrl, uploadInfo, fileName);
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

    }

}

