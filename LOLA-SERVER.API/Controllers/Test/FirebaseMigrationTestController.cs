using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LOLA_SERVER.API.Controllers.Test
{
    [ApiController]
    [Route("api/[controller]")]
    public class FirebaseMigrationTestController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public FirebaseMigrationTestController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet("project-info")]
        public IActionResult GetProjectInfo()
        {
            // Leer configuración completa de Firebase desde appsettings.json
            var firebaseConfig = _configuration.GetSection("Firebase");
            
            return Ok(new 
            { 
                message = "Información del proyecto Firebase",
                timestamp = DateTime.UtcNow,
                status = "Migración completada",
                firebaseConfiguration = new
                {
                    type = firebaseConfig["type"],
                    project_id = firebaseConfig["project_id"],
                    private_key_id = firebaseConfig["private_key_id"],
                    private_key = firebaseConfig["private_key"],
                    client_email = firebaseConfig["client_email"],
                    client_id = firebaseConfig["client_id"],
                    auth_uri = firebaseConfig["auth_uri"],
                    token_uri = firebaseConfig["token_uri"],
                    auth_provider_x509_cert_url = firebaseConfig["auth_provider_x509_cert_url"],
                    client_x509_cert_url = firebaseConfig["client_x509_cert_url"],
                    universe_domain = firebaseConfig["universe_domain"]
                }
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