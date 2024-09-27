using LOLA_SERVER.API.Models.PetService;

namespace LOLA_SERVER.API.Interfaces.Services
{
    public interface IPetServicesService
    {
        Task<(List<Caregiver> nearbyCaregivers, List<String> topics)> FindNearbyCaregivers(NearbyCaregiverRequest nearbyCaregiverRequest, string senderUser, string searchRadioId, string city, string typeServiceId, string BookingId);
        Task<(List<Caregiver> nearestCaregivers, List<Caregiver> nearCaregivers, List<Caregiver> cityCaregivers)> FindNearbyCaregiversByDistance(Coordinates coordinates, string typeServiceId, string city);
    }
}
