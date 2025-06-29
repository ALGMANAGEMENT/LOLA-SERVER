using Google.Apis.Auth.OAuth2;
using LOLA_SERVER.API.Interfaces.Services;
using System.Collections.Generic;
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
                    // Procesar el JSON para corregir el formato de la clave privada
                    var processedJson = ProcessFirebaseCredentialsJson(firebaseCredentialsJson);
                    return GoogleCredential.FromJson(processedJson);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al crear credenciales desde variable de entorno: {ex.Message}");
                    
                    // Intentar con el JSON original como fallback
                    try
                    {
                        return GoogleCredential.FromJson(firebaseCredentialsJson);
                    }
                    catch (Exception originalEx)
                    {
                        Console.WriteLine($"Error también con JSON original: {originalEx.Message}");
                    }
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

        private static string ProcessFirebaseCredentialsJson(string credentialsJson)
        {
            try
            {
                using (JsonDocument document = JsonDocument.Parse(credentialsJson))
                {
                    var root = document.RootElement;
                    
                    // Crear un nuevo objeto con la clave privada procesada
                    var processedCredentials = new Dictionary<string, object>();
                    
                    foreach (var property in root.EnumerateObject())
                    {
                        if (property.Name == "private_key" && property.Value.ValueKind == JsonValueKind.String)
                        {
                            var privateKey = property.Value.GetString();
                            processedCredentials[property.Name] = ProcessPrivateKey(privateKey);
                        }
                        else
                        {
                            processedCredentials[property.Name] = property.Value.ValueKind switch
                            {
                                JsonValueKind.String => property.Value.GetString(),
                                JsonValueKind.Number => property.Value.GetDecimal(),
                                JsonValueKind.True => true,
                                JsonValueKind.False => false,
                                JsonValueKind.Null => null,
                                _ => property.Value.GetRawText()
                            };
                        }
                    }
                    
                    return JsonSerializer.Serialize(processedCredentials);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al procesar credenciales JSON: {ex.Message}");
                return credentialsJson; // Retornar el original si falla el procesamiento
            }
        }

        private static string ProcessPrivateKey(string privateKey)
        {
            if (string.IsNullOrEmpty(privateKey))
                return privateKey;

            // Reemplazar \n con saltos de línea reales
            string processedKey = privateKey.Replace("\\n", "\n");
            
            // Verificar si ya tiene los headers correctos
            if (!processedKey.Contains("-----BEGIN PRIVATE KEY-----"))
            {
                // Si no tiene headers, agregarlos
                processedKey = processedKey.Trim();
                
                // Remover headers existentes si los hay (por si acaso)
                processedKey = processedKey
                    .Replace("-----BEGIN PRIVATE KEY-----", "")
                    .Replace("-----END PRIVATE KEY-----", "")
                    .Replace("-----BEGIN RSA PRIVATE KEY-----", "")
                    .Replace("-----END RSA PRIVATE KEY-----", "")
                    .Trim();
                
                // Agregar los headers correctos
                processedKey = $"-----BEGIN PRIVATE KEY-----\n{processedKey}\n-----END PRIVATE KEY-----";
            }
            
            return processedKey;
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