using LOLA_SERVER.API.Controllers.Base;
using LOLA_SERVER.API.Interfaces.Services;
using LOLA_SERVER.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LOLA_SERVER.API.Controllers.Images.v1
{
    [Authorize]
    [Route("api/v1/images")]
    [ApiController]
    public class ImagesController : BaseController
    {
        private readonly IImageService _imageService;

        public ImagesController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadImage(List<IFormFile> files, [FromQuery] string folder = "")
        {
            if (string.IsNullOrEmpty(folder))
                return ApiResponseError("Es necesario especificar una carpeta.");

            if (files == null || !files.Any())
                return ApiResponseError("No se ha proporcionado un archivo válido.");

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "test";

            try
            {
                var uploadedUrls = await _imageService.UploadImages(files, folder, userId);
                return ApiResponse(new { Urls = uploadedUrls });
            }
            catch (Exception ex)
            {
                return ApiResponseServerError($"Error interno al cargar la imagen: {ex.Message}");
            }
        }
    }
}