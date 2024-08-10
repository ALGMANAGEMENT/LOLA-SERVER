using LOLA_SERVER.API.Models.PetService;

namespace LOLA_SERVER.API.Interfaces.Services
{
    public interface IPetServicesService
    {
        Task<List<Caregiver>> FindNearbyCaregivers(Coordinates coordinates);
    }
}
