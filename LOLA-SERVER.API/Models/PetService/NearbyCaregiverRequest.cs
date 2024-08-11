using LOLA_SERVER.API.Models.Notifications;

namespace LOLA_SERVER.API.Models.PetService
{
    public class NearbyCaregiverRequest
    {
        public Coordinates Coordinates { get; set; }
        public NotificationRequest NotificationData { get; set; }
    }
}
