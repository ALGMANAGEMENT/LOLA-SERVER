﻿using LOLA_SERVER.API.Controllers.Base;
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
            [FromQuery, DefaultValue("OJKRlql5tLKB4eZNSjyF")] string BookingId,
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
                var (nearbyCaregivers, topics) = await _petServicesService.FindNearbyCaregivers(nearbyCaregiverRequest, senderUser, SearchRadioId, City, TypeService, BookingId);

                if (nearbyCaregiverRequest.NotificationData != null)
                {
                    string title = nearbyCaregiverRequest.NotificationData.Title ?? "New Pet Service Request";
                    string body = nearbyCaregiverRequest.NotificationData.Body ?? "A new pet service request is available near you!";
                    List<string> recipients = new List<string>();
       
                    foreach (var topic in topics)
                    {
                        try
                        {
                            var userId= topic.Split('-').Last();
                            recipients.Add(userId);
                            await _firebaseMessagingService.SendNotificationToTopicAsync(title, body, topic, senderUser, recipients);
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
                return ApiResponseServerError($"An error occurred while processing your request: {ex}");
            }
        }

        [HttpGet("nearby-caregivers")]
        public async Task<IActionResult> FindAllNearbyCaregivers(
             [FromQuery] double latitude =0,
            [FromQuery] double longitude=0,
            [FromQuery] string typeServiceId = "xk4keuRmmN9mFhRYC48D",
            [FromQuery] string city = "sogamoso")
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var coordinates = new Coordinates { Latitude = latitude, Longitude = longitude };
            if (coordinates == null)
            {
                return BadRequest("Coordinates are required");
            }
            try
            {
                var (nearestCaregivers, nearCaregivers, cityCaregivers) = await _petServicesService.FindNearbyCaregiversDirectly(
                    coordinates,
                    typeServiceId,
                    city
                );

                return Ok(new
                {
                    Message = "Caregivers found successfully.",
                    NearestCaregivers = new
                    {
                        Count = nearestCaregivers.Count,
                        Description = "Caregivers within 5 km",
                        Caregivers = nearestCaregivers
                    },
                    NearCaregivers = new
                    {
                        Count = nearCaregivers.Count,
                        Description = "Caregivers between 5 and 10 km",
                        Caregivers = nearCaregivers
                    },
                    CityCaregivers = new
                    {
                        Count = cityCaregivers.Count,
                        Description = $"Caregivers beyond 10 km but in {city}",
                        Caregivers = cityCaregivers
                    }
                });
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"An error occurred while processing your request: {ex.Message}");
            }
        }


    }
}