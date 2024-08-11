
namespace LOLA_SERVER.API.Interfaces.Services
{
    public interface INotificationsService
    {
        Task SendNotificationAsync(string title, string body, string token, string userId);
        Task SendNotificationToTopicAsync(string title, string body, string topic, string userId);
    }
}
