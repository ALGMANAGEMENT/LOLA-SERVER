using Google.Apis.Auth.OAuth2;
using Google.Apis.FirebaseCloudMessaging.v1.Data;
using Google.Apis.FirebaseCloudMessaging.v1;
using Google.Apis.Services;
using Google.Cloud.Firestore.V1;
using Google.Cloud.Firestore;
using LOLA_SERVER.API.Interfaces.Services;
using LOLA_SERVER.API.Models.Notifications;

namespace LOLA_SERVER.API.Services.NotificationsService
{
    public class NotificationsService : INotificationsService
    {

        private readonly FirebaseCloudMessagingService _firebaseMessagingService;
        private readonly FirestoreDb _firestoreDb;

        public NotificationsService()
        {
            // Inicializar el cliente de Firebase Messaging
            string credentialPath = "Utils/Credentials/Firebase-Credentials.json";
            var credential = GoogleCredential.FromFile(credentialPath);
            _firebaseMessagingService = new FirebaseCloudMessagingService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "LOLA_SERVER"
            });

            // Inicializar el cliente de Firestore
            var builder = new FirestoreClientBuilder
            {
                Credential = credential
            };
            _firestoreDb = FirestoreDb.Create("lola-app-e5f71", builder.Build());
        }

        /// <summary>
        /// Envía una notificación a un token específico y almacena la notificación en Firestore.
        /// </summary>
        public async Task SendNotificationAsync(string title, string body, string token, string userId)
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

            // Almacenar la notificación en Firestore
            await StoreNotificationAsync(new NotificationDBData
            {
                IdNotification = Guid.NewGuid().ToString(),
                Message = body,
                CreationDate = Timestamp.FromDateTime(DateTime.UtcNow),
                Read = false,
                Status = "Sent",
                Type = "Individual",
                TypeNotification = "General",
                User = userId,
                Topics = new List<string>() // No hay tópicos en una notificación a un token específico
            });
        }

        /// <summary>
        /// Envía una notificación a un tópico específico y almacena la notificación en Firestore.
        /// </summary>
        public async Task SendNotificationToTopicAsync(string title, string body, string topic, string userId)
        {
            try
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

                // Almacenar la notificación en Firestore
                var topicsArray = topic.Split('/');
                await StoreNotificationAsync(new NotificationDBData
                {
                    IdNotification = Guid.NewGuid().ToString(),
                    Message = body,
                    CreationDate = Timestamp.FromDateTime(DateTime.UtcNow),
                    Read = false,
                    Status = "Sent",
                    Type = "Topic",
                    TypeNotification = "General",
                    User = userId,
                    Topics = new List<string>(topicsArray)
                });
            }
            catch(Exception ex)
            {
                // Log the error but continue with other notifications
                Console.WriteLine($"Error sending notification to topic {topic}: {ex.Message}");
            }


           
        }

        /// <summary>
        /// Almacena la notificación en Firestore.
        /// </summary>
        private async Task StoreNotificationAsync(NotificationDBData notification)
        {
            try
            {
                CollectionReference collection = _firestoreDb.Collection("Notifications");
                await collection.AddAsync(notification);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
