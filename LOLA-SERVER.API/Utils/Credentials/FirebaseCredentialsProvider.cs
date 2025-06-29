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
                Console.WriteLine("Intentando usar credenciales desde variable de entorno FIREBASE_CREDENTIALS_JSON");
                try
                {
                    // Procesar el JSON para corregir el formato de la clave privada
                    var processedJson = ProcessFirebaseCredentialsJson(firebaseCredentialsJson);
                    Console.WriteLine("Credenciales procesadas exitosamente desde variable de entorno");
                    return GoogleCredential.FromJson(processedJson);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al crear credenciales desde variable de entorno: {ex.Message}");
                    
                    // Intentar con el JSON original como fallback
                    try
                    {
                        Console.WriteLine("Intentando con JSON original como fallback...");
                        return GoogleCredential.FromJson(firebaseCredentialsJson);
                    }
                    catch (Exception originalEx)
                    {
                        Console.WriteLine($"Error también con JSON original: {originalEx.Message}");
                    }
                }
            }
            else
            {
                Console.WriteLine("Variable de entorno FIREBASE_CREDENTIALS_JSON no encontrada");
            }

            // Intentar crear credenciales desde la configuración (appsettings.json)
            try
            {
                Console.WriteLine("Intentando crear credenciales desde configuración appsettings.json");
                var firebaseSection = configuration.GetSection("Firebase");
                if (firebaseSection.Exists())
                {
                    var credentialsJson = CreateJsonFromConfiguration(firebaseSection);
                    if (!string.IsNullOrEmpty(credentialsJson))
                    {
                        Console.WriteLine("Credenciales creadas exitosamente desde configuración");
                        return GoogleCredential.FromJson(credentialsJson);
                    }
                    else
                    {
                        Console.WriteLine("No se pudieron crear credenciales desde configuración - campos requeridos faltantes");
                    }
                }
                else
                {
                    Console.WriteLine("Sección Firebase no encontrada en configuración");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear credenciales desde configuración: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }

            // Fallback: usar archivo de credenciales
            var credentialPath = configuration["Firebase:CredentialsPath"] ?? "Utils/Credentials/Firebase-Credentials.json";
            Console.WriteLine($"Intentando usar archivo de credenciales: {credentialPath}");
            
            if (!File.Exists(credentialPath))
            {
                Console.WriteLine($"Archivo de credenciales no encontrado: {credentialPath}");
                throw new FileNotFoundException($"No se encontraron credenciales de Firebase. " +
                    $"Configura las credenciales en appsettings.json, proporciona FIREBASE_CREDENTIALS_JSON como variable de entorno, " +
                    $"o asegúrate de que el archivo existe en: {credentialPath}");
            }

            Console.WriteLine("Usando archivo de credenciales como fallback");
            return GoogleCredential.FromFile(credentialPath);
        }

        private static string ProcessFirebaseCredentialsJson(string credentialsJson)
        {
            try
            {
                Console.WriteLine("Procesando JSON de credenciales Firebase...");
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
                            Console.WriteLine($"Procesando clave privada (longitud: {privateKey?.Length})");
                            var processedKey = ProcessPrivateKey(privateKey);
                            processedCredentials[property.Name] = processedKey;
                            Console.WriteLine("Clave privada procesada exitosamente");
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
            {
                Console.WriteLine("Clave privada está vacía o es null");
                return privateKey;
            }

            Console.WriteLine($"Procesando clave privada original (longitud: {privateKey.Length})");
            Console.WriteLine($"Primeros 100 caracteres: {privateKey.Substring(0, Math.Min(100, privateKey.Length))}");

            // Manejar múltiples niveles de escape
            string processedKey = privateKey;
            
            // Manejar triple escape (\\\n -> \\n -> \n -> salto de línea)
            processedKey = processedKey.Replace("\\\\\\n", "\n");
            
            // Manejar doble escape (\\n -> \n)
            processedKey = processedKey.Replace("\\\\n", "\n");
            
            // Manejar escape simple (\n -> salto de línea real)
            processedKey = processedKey.Replace("\\n", "\n");
            
            // Limpiar espacios al inicio y final
            processedKey = processedKey.Trim();
            
            Console.WriteLine($"Después del procesamiento de escapes (longitud: {processedKey.Length})");
            Console.WriteLine($"Primeros 100 caracteres procesados: {processedKey.Substring(0, Math.Min(100, processedKey.Length))}");
            
            // Verificar si ya tiene los headers correctos
            if (!processedKey.Contains("-----BEGIN PRIVATE KEY-----"))
            {
                Console.WriteLine("La clave no tiene headers, agregándolos...");
                
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
            else
            {
                Console.WriteLine("La clave ya tiene headers correctos");
                
                // Asegurar que los saltos de línea estén correctos
                if (!processedKey.Contains("\n") && processedKey.Contains("\\n"))
                {
                    Console.WriteLine("Corrigiendo saltos de línea restantes...");
                    processedKey = processedKey.Replace("\\n", "\n");
                }
            }
            
            Console.WriteLine($"Clave privada final (longitud: {processedKey.Length})");
            Console.WriteLine($"Empieza con: {processedKey.Substring(0, Math.Min(50, processedKey.Length))}");
            Console.WriteLine($"Termina con: {processedKey.Substring(Math.Max(0, processedKey.Length - 50))}");
            
            return processedKey;
        }

        private static string? CreateJsonFromConfiguration(IConfigurationSection firebaseSection)
        {
            try
            {
                Console.WriteLine("Creando JSON desde configuración appsettings...");
                
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
                var missingFields = new List<string>();
                if (string.IsNullOrEmpty(credentials.type)) missingFields.Add("type");
                if (string.IsNullOrEmpty(credentials.project_id)) missingFields.Add("project_id");
                if (string.IsNullOrEmpty(credentials.private_key)) missingFields.Add("private_key");
                if (string.IsNullOrEmpty(credentials.client_email)) missingFields.Add("client_email");

                if (missingFields.Any())
                {
                    Console.WriteLine($"Campos requeridos faltantes en configuración: {string.Join(", ", missingFields)}");
                    return null;
                }

                // Procesar la clave privada si está presente
                var processedCredentials = new Dictionary<string, object>
                {
                    ["type"] = credentials.type,
                    ["project_id"] = credentials.project_id,
                    ["private_key_id"] = credentials.private_key_id,
                    ["private_key"] = ProcessPrivateKey(credentials.private_key),
                    ["client_email"] = credentials.client_email,
                    ["client_id"] = credentials.client_id,
                    ["auth_uri"] = credentials.auth_uri,
                    ["token_uri"] = credentials.token_uri,
                    ["auth_provider_x509_cert_url"] = credentials.auth_provider_x509_cert_url,
                    ["client_x509_cert_url"] = credentials.client_x509_cert_url,
                    ["universe_domain"] = credentials.universe_domain
                };

                var json = JsonSerializer.Serialize(processedCredentials);
                Console.WriteLine("JSON de credenciales creado exitosamente desde configuración");
                return json;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al serializar credenciales desde configuración: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return null;
            }
        }
    }
} 