using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Google.Cloud.Storage.V1;
using LOLA_SERVER.API.Interfaces.Services;
using LOLA_SERVER.API.Models.Images;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace LOLA_SERVER.API.Services
{
    public class ImageService : IImageService
    {
        private readonly StorageClient _storageClient;
        private readonly string _bucketName = "lola-manager.appspot.com";
        private readonly FirestoreDb _firestoreDb;

        public ImageService(IFirebaseCredentialsProvider credentialsProvider)
        {
            var credential = credentialsProvider.GetCredentials();
            _storageClient = StorageClient.Create(credential);
            var builder = new FirestoreClientBuilder
            {
                Credential = credential
            };
            _firestoreDb = FirestoreDb.Create("lola-manager", builder.Build());
        }

        public async Task<List<string>> UploadImages(List<IFormFile> files, string folder, string userId)
        {
            var uploadedUrls = new List<string>();

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var uniqueFileName = GenerateUniqueFileName(file);
                    var filePath = $"{SanitizeFolderPath($"{folder}")}/{uniqueFileName}";

                    using var stream = file.OpenReadStream();
                    var storageObject = await _storageClient.UploadObjectAsync(_bucketName, filePath, file.ContentType, stream, new UploadObjectOptions
                    {
                        PredefinedAcl = PredefinedObjectAcl.PublicRead
                    });

                    var publicUrl = $"https://storage.googleapis.com/{_bucketName}/{storageObject.Name}";
                    uploadedUrls.Add(publicUrl);
                    await SaveImageInfoToFirebase(new ImageInfoDB
                    {
                        FilePath = folder,
                        NameHash = uniqueFileName,
                        OriginalFileName = uniqueFileName,
                        CreatedDate = DateTime.UtcNow,
                        UploadDate = DateTime.UtcNow,
                        Url = publicUrl,
                        UserId = userId
                    });
                }
            }

            return uploadedUrls;
        }

        public async Task SaveImageInfoToFirebase(ImageInfoDB ImageInformation)
        {
            try
            {
                CollectionReference collection = _firestoreDb.Collection("Images");
                await collection.AddAsync(ImageInformation);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error saving image info to Firebase: {ex.Message}");
            }
        }

        public string SanitizeFolderPath(string PathFolder)
        {
            var sanitized = Regex.Replace(PathFolder, @"[^a-zA-Z0-9/]", "");
            sanitized = Regex.Replace(sanitized, @"/+", "/");
            return sanitized.Trim().TrimEnd('/').TrimStart('/');
        }

        public string GenerateUniqueFileName(IFormFile file)
        {
            using var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes($"{file.FileName}{DateTime.UtcNow.Ticks}"));
            var fileNameHash = BitConverter.ToString(hash).Replace("-", "").ToLower();
            var fileExtension = Path.GetExtension(file.FileName);
            return $"{fileNameHash}{fileExtension}";
        }
    }
}