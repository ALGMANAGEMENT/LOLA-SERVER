using Google.Apis.Auth.OAuth2;
using Google.Apis.FirebaseCloudMessaging.v1;
using Google.Apis.Services;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Google.Cloud.Storage.V1;
using LOLA_SERVER.API.Interfaces.Services;
using LOLA_SERVER.API.Models.Notifications;
using LOLA_SERVER.API.Models.PetService;
using Microsoft.AspNetCore.Mvc;

namespace LOLA_SERVER.API.Services.PetServicesService
{
    public class PetServicesService: IPetServicesService
    {
        private readonly FirestoreDb _firestoreDb;

        public PetServicesService()
        {
            string credentialPath = "Utils/Credentials/Firebase-Credentials.json";
            var credential = GoogleCredential.FromFile(credentialPath);
            var builder = new FirestoreClientBuilder
            {
                Credential = credential
            };
            _firestoreDb = FirestoreDb.Create("lola-app-e5f71", builder.Build());

        }

        public async Task<(List<Caregiver> nearbyCaregivers, List<String> topics)> FindNearbyCaregivers(NearbyCaregiverRequest nearbyCaregiverRequest, string SenderUser)
        {
            
            
            var coordinates = nearbyCaregiverRequest.Coordinates;
            var notificationData = nearbyCaregiverRequest.NotificationData;
            var usersCollection = _firestoreDb.Collection("Users");
            var query = usersCollection.WhereEqualTo("UserType", "care");
            var snapshot = await query.GetSnapshotAsync();
            var nearbyCaregivers = new List<Caregiver>();
            var radiusKm = 10d;
            var topics = new List<string>();
            foreach (var document in snapshot.Documents)
            {
                var userData = document.ToDictionary();
                if (userData.TryGetValue("Ubication", out object ubicationObj) &&
                    ubicationObj is Dictionary<string, object> ubication &&
                    ubication.TryGetValue("coordinates", out object coordinatesObj) &&
                    coordinatesObj is Dictionary<string, object> userCoordinates)
                {
                    if (userCoordinates.TryGetValue("latitude", out object latObj) &&
                        userCoordinates.TryGetValue("longitude", out object lonObj) &&
                        double.TryParse(latObj.ToString(), out double latitude) &&
                        double.TryParse(lonObj.ToString(), out double longitude))
                    {
                        var userCoords = new Coordinates { Latitude = latitude, Longitude = longitude };
                        if (IsWithinRadius(coordinates, userCoords, radiusKm))
                        {
                            var caregiver = Caregiver.FromDictionary(document.Id, userData);
                            nearbyCaregivers.Add(caregiver);
                            

                            // Crear el tópico
                            string topic = $"petservice/newrequest/care/{caregiver.Id}";
                            topics.Add(topic);
                        }
                    }
                }
            }



            return (nearbyCaregivers, topics);
        }




        private bool IsWithinRadius(Coordinates coord1, Coordinates coord2, Double radiusKm = 10)
        {
            const double EarthRadiusKm = 6371;

            var lat1 = DegreesToRadians(coord1.Latitude);
            var lon1 = DegreesToRadians(coord1.Longitude);
            var lat2 = DegreesToRadians(coord2.Latitude);
            var lon2 = DegreesToRadians(coord2.Longitude);

            var dLat = lat2 - lat1;
            var dLon = lon2 - lon1;

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(lat1) * Math.Cos(lat2) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            var distance = EarthRadiusKm * c;
            return distance <= radiusKm;
        }

        private double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }

    }
}
