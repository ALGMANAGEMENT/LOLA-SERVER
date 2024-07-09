using Google.Apis.Auth.OAuth2;
using Google.Apis.FirebaseCloudMessaging.v1;
using Google.Apis.FirebaseCloudMessaging.v1.Data;
using Google.Apis.Services;
using System.Threading.Tasks;

namespace LOLA_SERVER.API.Services
{
    public class FirebaseMessagingService
    {
        private readonly FirebaseCloudMessagingService _firebaseMessagingService;

        public FirebaseMessagingService()
        {
            // Inicializar el cliente de Firebase Messaging
            string credentialPath = "Utils/Credentials/Firebase-Credentials.json";
            var credential = GoogleCredential.FromFile(credentialPath);
            _firebaseMessagingService = new FirebaseCloudMessagingService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "LOLA_SERVER"
            });
        }

        public async Task SendNotificationAsync(string title, string body, string token)
        {
            var message = new Message
            {
                Token = token,
                Notification = new Notification
                {
                    Title = title,
                    Body = body
                }
            };

            var sendMessageRequest = new SendMessageRequest
            {
                Message = message
            };

            var request = _firebaseMessagingService.Projects.Messages.Send(sendMessageRequest, "projects/lola-app-e5f71");
            await request.ExecuteAsync();
        }

        /// <summary>
        /// Envía una notificación a un tópico específico.
        /// </summary>
        public async Task SendNotificationToTopicAsync(string title, string body, string topic)
        {
            var message = new Message
            {
                Topic = topic,
                Notification = new Notification
                {
                    Title = title,
                    Body = body
                }
            };

            var sendMessageRequest = new SendMessageRequest
            {
                Message = message
            };

            var request = _firebaseMessagingService.Projects.Messages.Send(sendMessageRequest, "projects/lola-app-e5f71");
            await request.ExecuteAsync();
        }
    }
}
