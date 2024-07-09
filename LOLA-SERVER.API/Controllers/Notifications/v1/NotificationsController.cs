using LOLA_SERVER.API.Controllers.Base;
using LOLA_SERVER.API.Models.Notifications;
using LOLA_SERVER.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LOLA_SERVER.API.Controllers.Notifications.v1
{
    //[Authorize]
    [Route("api/v1/notifications")]
    [ApiController]
    public class NotificationsController : BaseController
    {
        private readonly FirebaseMessagingService _firebaseMessagingService;

        public NotificationsController(FirebaseMessagingService firebaseMessagingService)
        {
            _firebaseMessagingService = firebaseMessagingService;
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await Task.FromResult(ApiResponse("Notifications OK"));
        }
        [Authorize]
        [HttpPost("send")]
        public async Task<IActionResult> SendNotification([FromBody] NotificationRequest request)
        {
            if (string.IsNullOrEmpty(request.Token) || string.IsNullOrEmpty(request.Title) || string.IsNullOrEmpty(request.Body))
                return ApiResponseError("Token, título y cuerpo de la notificación son requeridos.");

            try
            {
                await _firebaseMessagingService.SendNotificationAsync(request.Title, request.Body, request.Token);
                return ApiResponse("Notificación enviada exitosamente.");
            }
            catch (Exception ex)
            {
                return ApiResponseServerError($"Error interno al enviar la notificación: {ex.Message}");
            }
        }
        [Authorize]
        [HttpPost("send-to-topic")]
        public async Task<IActionResult> SendNotificationToTopic([FromBody] TopicNotificationRequest request)
        {
            if (string.IsNullOrEmpty(request.Topic) || string.IsNullOrEmpty(request.Title) || string.IsNullOrEmpty(request.Body))
                return ApiResponseError("Tópico, título y cuerpo de la notificación son requeridos.");

            try
            {
                await _firebaseMessagingService.SendNotificationToTopicAsync(request.Title, request.Body, request.Topic);
                return ApiResponse("Notificación enviada exitosamente.");
            }
            catch (Exception ex)
            {
                return ApiResponseServerError($"Error interno al enviar la notificación: {ex.Message}");
            }
        }



    }
}
