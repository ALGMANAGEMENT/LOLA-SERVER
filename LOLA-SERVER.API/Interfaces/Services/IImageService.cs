
using LOLA_SERVER.API.Models.Images;

namespace LOLA_SERVER.API.Interfaces.Services
{
    public interface IImageService
    {
        Task<List<string>> UploadImages(List<IFormFile> files, string folder, string userId);
        Task SaveImageInfoToFirebase(ImageInfoDB imageInformation);
        string SanitizeFolderPath(string pathFolder);
        string GenerateUniqueFileName(IFormFile file);
    }
}
