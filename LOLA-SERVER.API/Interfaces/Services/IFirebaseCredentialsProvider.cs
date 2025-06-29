using Google.Apis.Auth.OAuth2;

namespace LOLA_SERVER.API.Interfaces.Services
{
    public interface IFirebaseCredentialsProvider
    {
        GoogleCredential GetCredentials();
    }
} 