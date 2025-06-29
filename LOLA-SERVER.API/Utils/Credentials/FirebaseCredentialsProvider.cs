using Google.Apis.Auth.OAuth2;
using LOLA_SERVER.API.Interfaces.Services;
using System.Text.Json;

namespace LOLA_SERVER.API.Utils.Credentials
{
    public class FirebaseCredentialsProvider : IFirebaseCredentialsProvider
    {
        private readonly IConfiguration _configuration;

        public FirebaseCredentialsProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public GoogleCredential GetCredentials()
        {
            return GetCredentials(_configuration);
        }

        public static GoogleCredential GetCredentials(IConfiguration configuration)
        {
            // Primero intentar obtener credenciales desde variables de entorno
            var firebaseCredentialsJson = Environment.GetEnvironmentVariable("FIREBASE_CREDENTIALS_JSON");
            
            if (!string.IsNullOrEmpty(firebaseCredentialsJson))
            {
                try
                {
                    return GoogleCredential.FromJson(firebaseCredentialsJson);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al crear credenciales desde variable de entorno: {ex.Message}");
                }
            }

            // Intentar crear credenciales desde la configuración (appsettings.json)
            try
            {
                var firebaseSection = configuration.GetSection("Firebase");
                if (firebaseSection.Exists())
                {
                    var credentialsJson = CreateJsonFromConfiguration(firebaseSection);
                    if (!string.IsNullOrEmpty(credentialsJson))
                    {
                        return GoogleCredential.FromJson(credentialsJson);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear credenciales desde configuración: {ex.Message}");
            }

            // Fallback: usar archivo de credenciales
            var credentialPath = configuration["Firebase:CredentialsPath"] ?? "Utils/Credentials/Firebase-Credentials.json";
            
            if (!File.Exists(credentialPath))
            {
                throw new FileNotFoundException($"No se encontraron credenciales de Firebase. " +
                    $"Configura las credenciales en appsettings.json, proporciona FIREBASE_CREDENTIALS_JSON como variable de entorno, " +
                    $"o asegúrate de que el archivo existe en: {credentialPath}");
            }

            return GoogleCredential.FromFile(credentialPath);
        }

        private static string? CreateJsonFromConfiguration(IConfigurationSection firebaseSection)
        {
            try
            {
                // Crear el objeto de credenciales usando exactamente los nombres del appsettings.json
                var credentials = new
                {
                    type = firebaseSection["type"],
                    project_id = firebaseSection["project_id"],
                    private_key_id = firebaseSection["private_key_id"],
                    private_key = firebaseSection["private_key"],
                    client_email = firebaseSection["client_email"],
                    client_id = firebaseSection["client_id"],
                    auth_uri = firebaseSection["auth_uri"],
                    token_uri = firebaseSection["token_uri"],
                    auth_provider_x509_cert_url = firebaseSection["auth_provider_x509_cert_url"],
                    client_x509_cert_url = firebaseSection["client_x509_cert_url"],
                    universe_domain = firebaseSection["universe_domain"]
                };

                // Verificar que las propiedades esenciales estén presentes
                if (string.IsNullOrEmpty(credentials.type) || 
                    string.IsNullOrEmpty(credentials.project_id) || 
                    string.IsNullOrEmpty(credentials.private_key) || 
                    string.IsNullOrEmpty(credentials.client_email))
                {
                    return null;
                }

                return JsonSerializer.Serialize(credentials);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al serializar credenciales desde configuración: {ex.Message}");
                return null;
            }
        }
    }
} 