using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LOLA_SERVER.API.Controllers.Test
{
    [ApiController]
    [Route("api/[controller]")]
    public class FirebaseMigrationTestController : ControllerBase
    {
        [HttpGet("project-info")]
        public IActionResult GetProjectInfo()
        {
            return Ok(new 
            { 
                message = "Información del proyecto Firebase",
                projectId = "lola-manager",
                timestamp = DateTime.UtcNow,
                status = "Migración completada"
            });
        }

        [HttpGet("public")]
        public IActionResult PublicEndpoint()
        {
            return Ok(new { 
                message = "Endpoint público - Migración a lola-manager completada", 
                timestamp = DateTime.UtcNow 
            });
        }

        [HttpGet("protected")]
        [Authorize]
        public IActionResult ProtectedEndpoint()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            
            return Ok(new 
            { 
                message = "Endpoint protegido - Autenticación Firebase funcionando",
                projectId = "lola-manager",
                userId = userId,
                email = email,
                timestamp = DateTime.UtcNow,
                claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList()
            });
        }

        [HttpGet("firebase-config")]
        public IActionResult GetFirebaseConfig()
        {
            return Ok(new 
            { 
                projectId = "lola-manager",
                authDomain = "lola-manager.firebaseapp.com",
                authority = "https://securetoken.google.com/lola-manager",
                validAudiences = new[]
                {
                    "lola-manager",
                    "358545804776-8givt8sn6ni9lb1taf18svjc5nov59jt.apps.googleusercontent.com",
                    "358545804776-49en63dntddtsvaimgiujl26vm253f09.apps.googleusercontent.com"
                },
                timestamp = DateTime.UtcNow
            });
        }
    }
} 