using LOLA_SERVER.API.Controllers.Base;
using LOLA_SERVER.API.Interfaces.Services;
using LOLA_SERVER.API.Models.Notifications;
using LOLA_SERVER.API.Models.PetService;
using LOLA_SERVER.API.Services.MessagingService;
using LOLA_SERVER.API.Services.NotificationsService;
using LOLA_SERVER.API.Utils.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel;
using System.Security.Claims;

namespace LOLA_SERVER.API.Controllers.PetServices.V1
{
    //[Authorize]
    [Route("api/v1/pet-service")]
    [ApiController]
    [Produces("application/json")]
    public class PetServicesController : BaseController
    {
        private readonly IPetServicesService _petServicesService;
        private readonly FirebaseMessagingService _firebaseMessagingService;

        public PetServicesController(IPetServicesService petServicesService, FirebaseMessagingService firebaseMessagingService)
        {
            _petServicesService = petServicesService;
            _firebaseMessagingService = firebaseMessagingService;
        }

        /// <summary>
        /// Busca cuidadores cercanos basado en las coordenadas proporcionadas y envía notificaciones.
        /// </summary>
        /// <param name="City">La ciudad donde se realiza la búsqueda.</param>
        /// <param name="SearchRadioId">El ID del radio de búsqueda en Firebase.</param>
        /// <param name="nearbyCaregiverRequest">Los detalles de la solicitud de búsqueda.</param>
        /// <returns>Una lista de cuidadores cercanos y un mensaje de confirmación.</returns>
        /// <response code="200">Retorna la lista de cuidadores encontrados.</response>
        /// <response code="400">Si las coordenadas no se proporcionan o son inválidas.</response>
        /// <response code="500">Si ocurre un error interno del servidor.</response>
        [HttpPost("find-nearby-caregivers")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> FindNearbyCaregivers(
            [FromQuery, DefaultValue("sogamoso")] string City,
            [FromQuery, DefaultValue("xk4keuRmmN9mFhRYC48D")] string TypeService,
            [FromQuery, DefaultValue("roHMfAuwRi0NjYV2LwEz")] string SearchRadioId,
            [FromBody] NearbyCaregiverRequest nearbyCaregiverRequest)
        {
            if (nearbyCaregiverRequest.Coordinates == null)
            {
                return ApiResponseError("Coordinates are required");
            }
            if (TypeService.IsNullOrEmpty())
            {
                return ApiResponseError("Type Opportunity are required");
            }

            try
            {
                var senderUser = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "userNotifications";
                var (nearbyCaregivers, topics) = await _petServicesService.FindNearbyCaregivers(nearbyCaregiverRequest, senderUser, SearchRadioId, City, TypeService);

                if (nearbyCaregiverRequest.NotificationData != null)
                {
                    string title = nearbyCaregiverRequest.NotificationData.Title ?? "New Pet Service Request";
                    string body = nearbyCaregiverRequest.NotificationData.Body ?? "A new pet service request is available near you!";

                    foreach (var topic in topics)
                    {
                        try
                        {
                            await _firebaseMessagingService.SendNotificationToTopicAsync(title, body, topic, senderUser);
                        }
                        catch (Exception ex)
                        {
                            // Log the error but continue with other notifications
                            Console.WriteLine($"Error sending notification to topic {topic}: {ex.Message}");
                        }
                    }
                }

                return ApiResponse(new
                {
                    Message = $"Found {nearbyCaregivers.Count} caregivers.",
                    Caregivers = nearbyCaregivers
                });
            }
            catch (Exception ex)
            {
                // Log the exception
                return ApiResponseServerError("An error occurred while processing your request");
            }
        }
    }
}