using LOLA_SERVER.API.Controllers.Base;
using LOLA_SERVER.API.Models.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Security.Claims;
using LOLA_SERVER.API.Services.MessagingService;

namespace LOLA_SERVER.API.Controllers.Notifications.v1
{
    [Authorize]
    [Route("api/v1/notifications")]
    [ApiController]
    public class NotificationsController : BaseController
    {
        private readonly FirebaseMessagingService _firebaseMessagingService;

        public NotificationsController(FirebaseMessagingService firebaseMessagingService)
        {
            _firebaseMessagingService = firebaseMessagingService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await Task.FromResult(ApiResponse("Notifications OK"));
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendNotification([FromBody] NotificationRequest request)
        {
            if (string.IsNullOrEmpty(request.Token))
                return ApiResponseError("Token de la notificación es requerido.");

            if (string.IsNullOrEmpty(request.Title) || string.IsNullOrEmpty(request.Body))
                return ApiResponseError("Título y cuerpo de la notificación son requeridos.");

            try
            {
                // Obtener el userId del contexto de usuario autenticado
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "test";

                await _firebaseMessagingService.SendNotificationAsync(request.Title, request.Body, request.Token, userId);

                return ApiResponse("Notificación enviada exitosamente.");
            }
            catch (Exception ex)
            {
                return ApiResponseServerError($"Error interno al enviar la notificación: {ex.Message}");
            }
        }

        [HttpPost("send-to-city/{city}")]
        public async Task<IActionResult> SendNotificationToCity([FromBody] NotificationRequest request, string city)
        {
            if (string.IsNullOrEmpty(request.Title) || string.IsNullOrEmpty(request.Body))
                return ApiResponseError("Título y cuerpo de la notificación son requeridos.");

            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "test";
                var topic = $"{city}-";

                await _firebaseMessagingService.SendNotificationToTopicAsync(request.Title, request.Body, topic, userId);

                return ApiResponse("Notificación enviada a cuidadores en la ciudad exitosamente.");
            }
            catch (Exception ex)
            {
                return ApiResponseServerError($"Error interno al enviar la notificación: {ex.Message}");
            }
        }

        [HttpPost("send-to-admin")]
        public async Task<IActionResult> SendNotificationToAdmin([FromBody] NotificationRequest request)
        {
            if (string.IsNullOrEmpty(request.Title) || string.IsNullOrEmpty(request.Body))
                return ApiResponseError("Título y cuerpo de la notificación son requeridos.");

            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "test";
                var topic = "admin";

                await _firebaseMessagingService.SendNotificationToTopicAsync(request.Title, request.Body, topic, userId);

                return ApiResponse("Notificación enviada a administradores exitosamente.");
            }
            catch (Exception ex)
            {
                return ApiResponseServerError($"Error interno al enviar la notificación: {ex.Message}");
            }
        }

        [HttpPost("send-to-client/{clientId}")]
        public async Task<IActionResult> SendNotificationToClient([FromBody] NotificationRequest request, string clientId)
        {
            if (string.IsNullOrEmpty(request.Title) || string.IsNullOrEmpty(request.Body))
                return ApiResponseError("Título y cuerpo de la notificación son requeridos.");

            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "test";
                var topic = $"client-{clientId}";

                await _firebaseMessagingService.SendNotificationToTopicAsync(request.Title, request.Body, topic, userId);

                return ApiResponse("Notificación enviada al cliente exitosamente.");
            }
            catch (Exception ex)
            {
                return ApiResponseServerError($"Error interno al enviar la notificación: {ex.Message}");
            }
        }


        [HttpPost("send-multi-users")]
        public async Task<IActionResult> SendNotificationToUsers([FromBody] MultiUserNotificationRequest request)
        {
            if (request.UserIds == null || !request.UserIds.Any())
                return BadRequest("At least one user ID is required.");
            if (string.IsNullOrEmpty(request.Title) || string.IsNullOrEmpty(request.Body))
                return BadRequest("Title and body of the notification are required.");

            var successCount = 0;
            var failureCount = 0;
            var senderId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "system";

            try
            {
                foreach (var userId in request.UserIds)
                {
                    var topic = $"client-{userId}";
                    try
                    {
                        await _firebaseMessagingService.SendNotificationToTopicAsync(request.Title, request.Body, topic, senderId);
                        successCount++;
                    }
                    catch (Exception)
                    {
                        failureCount++;
                    }
                }

                return Ok(new { Message = "Notifications sent.", SuccessCount = successCount, FailureCount = failureCount });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error when sending notifications: {ex.Message}");
            }
        }

    }
}
