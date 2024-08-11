using LOLA_SERVER.API.Controllers.Base;
using LOLA_SERVER.API.Interfaces.Services;
using LOLA_SERVER.API.Models.Notifications;
using LOLA_SERVER.API.Models.PetService;
using LOLA_SERVER.API.Services.MessagingService;
using LOLA_SERVER.API.Services.NotificationsService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LOLA_SERVER.API.Controllers.PetServices.V1
{
    //[Authorize]
    [Route("api/v1/pet-service")]
    [ApiController]
    public class PetServicesController: BaseController
    {
        readonly IPetServicesService _petServicesService;
        private readonly FirebaseMessagingService _firebaseMessagingService;
        public PetServicesController(IPetServicesService petServicesService, FirebaseMessagingService firebaseMessagingService)
        {
            _petServicesService = petServicesService;
            _firebaseMessagingService = firebaseMessagingService;
        }

        [HttpPost("find-nearby-caregivers")]
        public async Task<IActionResult> FindNearbyCaregivers([FromBody] NearbyCaregiverRequest nearbyCaregiverRequest)
        {
            var coordinates = nearbyCaregiverRequest.Coordinates;
            if (coordinates == null)
            {
                return BadRequest("Coordinates are required");
            }

            try
            {
                var SenderUser = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "userNotifications";
                var (nearbyCaregivers, topics) = await _petServicesService.FindNearbyCaregivers(nearbyCaregiverRequest, SenderUser);

                var notificationData = nearbyCaregiverRequest.NotificationData;

                if (notificationData != null)
                {
                    string title = notificationData.Title ?? "New Pet Service Request";
                    string body = notificationData.Body ?? "A new pet service request is available near you!";

                    foreach (var topic in topics)
                    {
                        try
                        {
                            await _firebaseMessagingService.SendNotificationToTopicAsync(title, body, topic, SenderUser);
                        }
                        catch (Exception ex)
                        {
                            // Log the error but continue with other notifications
                            Console.WriteLine($"Error sending notification to topic {topic}: {ex.Message}");
                        }
                    }
                }

                return Ok(new
                {
                    Message = $"Found {nearbyCaregivers.Count} caregivers within  km.",
                    Caregivers = nearbyCaregivers
                });
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while processing your request");
            }
        }


    }
}
