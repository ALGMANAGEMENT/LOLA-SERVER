using LOLA_SERVER.API.Models.PetService;
using LOLA_SERVER.API.Models.Users;

namespace LOLA_SERVER.API.Interfaces.Services
{
    public interface IUsersService
    {
        Task<List<User>> GenerateAndSaveDummyUsers(int count, Coordinates coordinates);
        
    }
}
