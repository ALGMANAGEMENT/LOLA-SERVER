using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Linq;

namespace LOLA_SERVER.API.Controllers.Images.v1
{
    [Route("api/v1/images")]
    [ApiController]
    public class ImagesController : ControllerBase
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
        public async Task<IActionResult> UploadImage(IFormFile file, [FromQuery] string folder = "")
        {
            if (string.IsNullOrEmpty(folder))
                return BadRequest("Es necesario especificar una carpeta.");

            // Validar las carpetas permitidas
            var allowedFolders = new[] { "pets", "users", "histories" };
            if (!allowedFolders.Contains(folder.ToLower()))
                return BadRequest("La carpeta especificada no es válida.");

            if (file == null || file.Length == 0)
                return BadRequest("No se ha proporcionado un archivo válido.");

            try
            {
                var fileName = $"{folder}/{DateTime.UtcNow.Ticks}_{file.FileName}";

                using var stream = file.OpenReadStream();
                var storageObject = await _storageClient.UploadObjectAsync(_bucketName, fileName, file.ContentType, stream, new UploadObjectOptions
                {
                    PredefinedAcl = PredefinedObjectAcl.PublicRead
                });

                var publicUrl = $"https://storage.googleapis.com/{_bucketName}/{storageObject.Name}";

                return Ok(new { Url = publicUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno al cargar la imagen: {ex.Message}");
            }
        }
    }

}

