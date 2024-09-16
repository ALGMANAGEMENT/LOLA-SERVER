using Google;
using Google.Apis.Auth.OAuth2;
using Google.Apis.FirebaseCloudMessaging.v1;
using Google.Apis.FirebaseCloudMessaging.v1.Data;
using Google.Apis.Services;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using LOLA_SERVER.API.Models.Notifications;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LOLA_SERVER.API.Services.MessagingService
{
    public class FirebaseMessagingService
    {
        private readonly FirebaseCloudMessagingService _firebaseMessagingService;
        private readonly FirestoreDb _firestoreDb;

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
                Topics = new List<string>(),
                Recipients = new List<string>() { "ALL" },
                Title = title,
            });
        }

        /// <summary>
        /// Envía una notificación a un tópico específico y almacena la notificación en Firestore.
        /// </summary>
        public async Task SendNotificationToTopicAsync(string title, string body, string topic, string userId, List<string> recipients )
        {

            try
            {
                var formattedTopic = topic.Trim('/');
                formattedTopic = formattedTopic.StartsWith("topics-") ? formattedTopic : $"topics-{formattedTopic}";
                formattedTopic = $"{formattedTopic}".ToLower();
                var message = new Message
                {
                    Topic = formattedTopic,
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
                    Topics = new List<string>(topicsArray),
                    Title = title,
                    Recipients = recipients
                });
            }
            catch (GoogleApiException gex)
            {
                Console.WriteLine($"Google API Error sending notification to topic {topic}:");
                Console.WriteLine($"Error code: {gex.Error.Code}");
                Console.WriteLine($"Error message: {gex.Error.Message}");
                Console.WriteLine($"Error details: {gex.Error.ToString()}");
            }
            catch (Exception ex)
            {
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
                CollectionReference collection = _firestoreDb.Collection("notifications");
                await collection.AddAsync(notification);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
