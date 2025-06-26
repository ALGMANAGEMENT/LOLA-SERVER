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
using static Google.Api.ResourceDescriptor.Types;

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
            _firestoreDb = FirestoreDb.Create("lola-manager", builder.Build());
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

            var request = _firebaseMessagingService.Projects.Messages.Send(sendMessageRequest, "projects/lola-manager");
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

            var style = new NotificationStyle
            {
                Icon = "ic_notification",
                Color = "#FF5733",
                Sound = "notification_sound",
                ChannelId = "default_channel",
                Badge = 1,
                Image = "https://media.istockphoto.com/id/1186734274/vector/vector-image-of-dog-and-cat-logo-on-white.jpg?s=612x612&w=0&k=20&c=p_wU6nJyvY_31FlX4MSo8MMH1hBHcFo5HxutZccwL4c=",
                BigText = "Este es un texto más largo que se mostrará cuando la notificación se expanda.",
                SubText = "Información adicional"
            };
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
                    },
                    Android = new AndroidConfig
                    {
                        Notification = new AndroidNotification
                        {
                            Icon = style.Icon,
                            Color = style.Color,
                            Sound = style.Sound,
                            ChannelId = style.ChannelId
                        }
                    },
                    Data = new Dictionary<string, string>
                    {
                        { "image", style.Image },
                        { "bigText", style.BigText },
                        { "subText", style.SubText }
                    }
                };

                var sendMessageRequest = new SendMessageRequest
                {
                    Message = message
                };

                
                await SendDemoNotification(formattedTopic);
                var request = _firebaseMessagingService.Projects.Messages.Send(sendMessageRequest, "projects/lola-manager");
                await request.ExecuteAsync();

                // Almacenar la notificación en Firestore
                var topicsArray = topic.Split('-');
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


        private async Task SendDemoNotification(string topic)
        {
            var demoStyle = new NotificationStyle
            {
                Icon = "ic_demo_notification",
                Color = "#4CAF50",
                Sound = "demo_sound",
                ChannelId = "demo_channel",
                Badge = 2,
                Image = "https://images.unsplash.com/photo-1501504905252-473c47e087f8?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1074&q=80",
                BigText = "Esta es una notificación de demostración con texto expandible. Muestra cómo puedes incluir más contenido en tu notificación.",
                SubText = "Demo Notification"
            };

            var demoMessage = new Message
            {
                Topic = topic,
                Notification = new Notification
                {
                    Title = "Demo: Nueva Actualización",
                    Body = "¡Descubre las nuevas funciones de nuestra app!"
                },
                Android = new AndroidConfig
                {
                    Notification = new AndroidNotification
                    {
                        Icon = demoStyle.Icon,
                        Color = demoStyle.Color,
                        Sound = demoStyle.Sound,
                        ChannelId = demoStyle.ChannelId,
                        ClickAction = "OPEN_DEMO_ACTIVITY"
                    }
                },
                Data = new Dictionary<string, string>
                {
                    { "type", "demo" },
                    { "image", demoStyle.Image },
                    { "bigText", demoStyle.BigText },
                    { "subText", demoStyle.SubText },
                    { "actionButton1", "Ver Ahora" },
                    { "actionButton2", "Recordar Más Tarde" }
                }
            };

            var sendMessageRequest = new SendMessageRequest
            {
                Message = demoMessage
            };
            
            var request = _firebaseMessagingService.Projects.Messages.Send(sendMessageRequest, "projects/lola-manager");
            await request.ExecuteAsync();
           
        }


        private string FormatTopic(string topic)
        {
            var formattedTopic = topic.Trim('/');
            formattedTopic = formattedTopic.StartsWith("topics-") ? formattedTopic : $"topics-{formattedTopic}";
            return $"{formattedTopic}".ToLower();
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
