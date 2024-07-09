using LOLA_SERVER.API.Controllers.Base;
using LOLA_SERVER.API.Models.Notifications;
using LOLA_SERVER.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }

   

}
