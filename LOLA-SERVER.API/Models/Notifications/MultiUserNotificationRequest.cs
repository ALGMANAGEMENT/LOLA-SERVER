namespace LOLA_SERVER.API.Models.Notifications
{
    public class MultiUserNotificationRequest
    {
        public List<string> UserIds { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }

        public string Token { get; set; }
    }
}
