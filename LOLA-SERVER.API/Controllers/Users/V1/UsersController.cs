using LOLA_SERVER.API.Interfaces.Services;
using LOLA_SERVER.API.Models.PetService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LOLA_SERVER.API.Controllers.Users.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        readonly IUsersService _usersService;
        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpPost("generate-dummy-users")]
        public async Task<IActionResult> GenerateDummyUsers([FromBody] Coordinates coordinates = null,
            [FromQuery] int count = 10)
        {
            if (count <= 0 || count > 100)
            {
                return BadRequest("Count must be between 1 and 100.");
            }

            var users = await _usersService.GenerateAndSaveDummyUsers(count, coordinates);
            return Ok(new { Message = $"{count} dummy users generated and saved.", Users = users });
        }

    }
}
