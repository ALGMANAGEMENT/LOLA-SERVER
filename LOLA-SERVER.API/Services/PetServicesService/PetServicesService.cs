using Google.Apis.Auth.OAuth2;
using Google.Apis.FirebaseCloudMessaging.v1;
using Google.Apis.Services;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Google.Cloud.Storage.V1;
using LOLA_SERVER.API.Interfaces.Services;
using LOLA_SERVER.API.Models.Notifications;
using LOLA_SERVER.API.Models.PetService;
using LOLA_SERVER.API.Models.Users;
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

        public async Task<(List<Caregiver> nearbyCaregivers, List<string> topics)> FindNearbyCaregivers(NearbyCaregiverRequest nearbyCaregiverRequest,
            string senderUser, string searchRadioId, string city, string typeServiceId, string BookingId)
        {
            var coordinates = nearbyCaregiverRequest.Coordinates;
            var radiusKm = await GetRadiusKmFromFirebase(searchRadioId);
            var typeServiceSnapshot = await _firestoreDb.Collection("TypesServices").Document(typeServiceId).GetSnapshotAsync();
            var typeService = typeServiceSnapshot.ConvertTo<TypeService>();
            var typeServiceRequired = typeService.value;
            var servicesCollection = _firestoreDb.Collection("services");
            var servicesQuery = servicesCollection.WhereEqualTo("ServiceSelected", typeServiceRequired);
            var servicesSnapshot = await servicesQuery.GetSnapshotAsync();
            var bookingSnapshot = await _firestoreDb.Collection("bookings").Document(BookingId).GetSnapshotAsync();
            var booking = bookingSnapshot.ConvertTo<Booking>();

            var nearbyCaregivers = new List<Caregiver>();
            var topics = new List<string>();

            if (radiusKm == 0)
            {
                // Search in the entire city
                topics.Add($"petservice-newrequest-care-{city}");
                foreach (var document in servicesSnapshot.Documents)
                {
                    var service = document.ConvertTo<Service>();
                    var caregiver = await GetCaregiverFromUser(service.IdUser);
                    if (caregiver != null)
                    {
                        nearbyCaregivers.Add(caregiver);
                    }
                }
            }
            else
            {
                // Search by radius
                foreach (var document in servicesSnapshot.Documents)
                {
                    var service = document.ConvertTo<Service>();
                    if (service.Location != null &&
                        service.Location.TryGetValue("coordinates", out object coordinatesObj) &&
                        coordinatesObj is Dictionary<string, object> serviceCoordinates)
                    {
                        if (serviceCoordinates.TryGetValue("latitude", out object latObj) &&
                            serviceCoordinates.TryGetValue("longitude", out object lonObj))
                        {
                            var serviceCoords = new Coordinates
                            {
                                Latitude = Convert.ToDouble(latObj),
                                Longitude = Convert.ToDouble(lonObj)
                            };
                            if (IsWithinRadius(coordinates, serviceCoords, radiusKm))
                            {
                                var caregiver = await GetCaregiverFromUser(service.IdUser);
                                if (caregiver != null)
                                {
                                    nearbyCaregivers.Add(caregiver);
                                    string topic = $"petservice-newrequest-care-{service.IdUser}";
                                    topics.Add(topic);
                                }
                            }
                        }
                    }
                }
            }


            foreach (var caregiver in nearbyCaregivers)
            {
                if (!booking.UrlCares.Contains(caregiver.Id))
                {
                    booking.UrlCares.Add(caregiver.Id);
                }
            }

            // Update the booking document in Firestore
            await _firestoreDb.Collection("bookings").Document(BookingId).SetAsync(booking, SetOptions.MergeAll);


            return (nearbyCaregivers, topics);
        }

        private async Task<Caregiver> GetCaregiverFromUser(string userId)
        {
            var userDoc = await _firestoreDb.Collection("users").Document(userId).GetSnapshotAsync();
            if (userDoc.Exists)
            {
                var userData = userDoc.ToDictionary();
                if (userData.TryGetValue("UserType", out object userTypeObj) &&
                    userTypeObj.ToString().Equals("care", StringComparison.OrdinalIgnoreCase))
                {
                    if (userData.ContainsKey("Ubication"))
                    {
                        userData.Remove("Ubication");
                    }

                    return Caregiver.FromDictionary(userId, userData);
                }
            }
            return null;
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
