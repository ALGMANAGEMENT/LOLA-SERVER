using LOLA_SERVER.API.Controllers.Base;
using LOLA_SERVER.API.Interfaces.Services;
using LOLA_SERVER.API.Models.PetService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LOLA_SERVER.API.Controllers.PetServices.V1
{
    //[Authorize]
    [Route("api/v1/pet-service")]
    [ApiController]
    public class PetServicesController: BaseController
    {
        readonly IPetServicesService _petServicesService;
        public PetServicesController(IPetServicesService petServicesService)
        {
            _petServicesService = petServicesService;
        }

        [HttpPost("find-nearby-caregivers")]
        public async Task<IActionResult> FindNearbyCaregivers([FromBody] Coordinates coordinates)
        {
            if (coordinates == null)
            {
                return BadRequest("Coordinates are required");
            }

            try
            {
                var nearbyCaregivers = await _petServicesService.FindNearbyCaregivers(coordinates);
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
