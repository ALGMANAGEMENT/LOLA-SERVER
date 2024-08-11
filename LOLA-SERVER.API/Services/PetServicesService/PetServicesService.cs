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

        public async Task<(List<Caregiver> nearbyCaregivers, List<string> topics)> FindNearbyCaregivers(NearbyCaregiverRequest nearbyCaregiverRequest, string senderUser, string searchRadioId, string city)
        {
            var coordinates = nearbyCaregiverRequest.Coordinates;
            var radiusKm = await GetRadiusKmFromFirebase(searchRadioId);

            var usersCollection = _firestoreDb.Collection("Users");
            var query = usersCollection.WhereEqualTo("UserType", "care");
            var snapshot = await query.GetSnapshotAsync();

            var nearbyCaregivers = new List<Caregiver>();
            var topics = new List<string>();

            if (radiusKm == 0)
            {
                // Búsqueda en toda la ciudad
                topics.Add($"petservice-newrequest-care-{city}");
                foreach (var document in snapshot.Documents)
                {
                    var caregiver = Caregiver.FromDictionary(document.Id, document.ToDictionary());
                    nearbyCaregivers.Add(caregiver);
                }
            }
            else
            {
                // Búsqueda por radio
                foreach (var document in snapshot.Documents)
                {
                    var userData = document.ToDictionary();
                    if (TryGetUserCoordinates(userData, out var userCoords))
                    {
                        if (IsWithinRadius(coordinates, userCoords, radiusKm))
                        {
                            var caregiver = Caregiver.FromDictionary(document.Id, userData);
                            nearbyCaregivers.Add(caregiver);
                            string topic = $"petservice-newrequest-care-{caregiver.Id}";
                            topics.Add(topic);
                        }
                    }
                }
            }

            return (nearbyCaregivers, topics);
        }

        private async Task<double> GetRadiusKmFromFirebase(string searchRadioId)
        {
            var radiusRequestDoc = await _firestoreDb.Collection("RadiusRequests").Document(searchRadioId).GetSnapshotAsync();
            if (radiusRequestDoc.Exists)
            {
                var radiusData = radiusRequestDoc.ToDictionary();
                if (radiusData.TryGetValue("radiusKm", out object radiusObj) && double.TryParse(radiusObj.ToString(), out double radiusKm))
                {
                    return radiusKm;
                }
            }
            throw new ArgumentException("Invalid search radio ID or missing radiusKm value");
        }

        private bool TryGetUserCoordinates(Dictionary<string, object> userData, out Coordinates coordinates)
        {
            coordinates = null;
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
                    coordinates = new Coordinates { Latitude = latitude, Longitude = longitude };
                    return true;
                }
            }
            return false;
        }

        private bool IsWithinRadius(Coordinates pointA, Coordinates pointB, double maxDistanceKm)
        {
            const double EarthRadiusKm = 6371; // Radio medio de la Tierra en kilómetros

            // Convertir coordenadas de grados a radianes
            var latitudeA = DegreesToRadians(pointA.Latitude);
            var longitudeA = DegreesToRadians(pointA.Longitude);
            var latitudeB = DegreesToRadians(pointB.Latitude);
            var longitudeB = DegreesToRadians(pointB.Longitude);

            // Calcular las diferencias entre las coordenadas
            var deltaLatitude = latitudeB - latitudeA;
            var deltaLongitude = longitudeB - longitudeA;

            // Aplicar la fórmula del haversine
            var haversineSquareHalf = Math.Sin(deltaLatitude / 2) * Math.Sin(deltaLatitude / 2) +
                                      Math.Cos(latitudeA) * Math.Cos(latitudeB) *
                                      Math.Sin(deltaLongitude / 2) * Math.Sin(deltaLongitude / 2);

            // Calcular el ángulo central
            var centralAngle = 2 * Math.Atan2(Math.Sqrt(haversineSquareHalf), Math.Sqrt(1 - haversineSquareHalf));

            // Calcular la distancia sobre la superficie de la Tierra
            var distanceKm = EarthRadiusKm * centralAngle;

            // Determinar si la distancia está dentro del radio máximo especificado
            return distanceKm <= maxDistanceKm;
        }
        private double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }
    }
}
