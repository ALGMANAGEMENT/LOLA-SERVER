namespace LOLA_SERVER.API.Models.Notifications
{
    public class TopicNotificationRequest
    {
        public string Token { get; set; }

        public string Topic { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }

    }
}
