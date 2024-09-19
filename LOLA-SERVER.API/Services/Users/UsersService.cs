using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using LOLA_SERVER.API.Interfaces.Services;
using LOLA_SERVER.API.Models.PetService;
using LOLA_SERVER.API.Models.Users;

namespace LOLA_SERVER.API.Services.Users
{
    public class UsersService: IUsersService
    {

        private readonly Random _random = new Random();
        private readonly FirestoreDb _firestoreDb;
        private Coordinates _centerCoordinates;
        public UsersService( )
        {
            string credentialPath = "Utils/Credentials/Firebase-Credentials.json";
            var credential = GoogleCredential.FromFile(credentialPath);
            var builder = new FirestoreClientBuilder
            {
                Credential = credential
            };
            _firestoreDb = FirestoreDb.Create("lola-app-e5f71", builder.Build());
        }

        public async Task<List<User>> GenerateAndSaveDummyUsers(int count, Coordinates coordinates)
        {
            var users = new List<User>();
            _centerCoordinates = coordinates;
            for (int i = 0; i < count; i++)
            {
                var user = GenerateDummyUser();
                users.Add(user);
                await SaveUserToFirestore(user);
                await GenerateAndSaveRandomService(user.Id);
            }
            return users;
        }

        public Dictionary<string, object> GenerateRandomUbication()
        {
            const double radiusKm = 10;
            const double earthRadiusKm = 6371;

            // Convert radius from kilometers to radians
            double radiusRadians = radiusKm / earthRadiusKm;

            // Generate a random angle and distance within the circle
            double randomAngle = _random.NextDouble() * 2 * Math.PI;
            double randomDistance = Math.Sqrt(_random.NextDouble()) * radiusRadians;

            // Convert center point to radians
            double centerLatRadians = _centerCoordinates.Latitude * Math.PI / 180;
            double centerLonRadians = _centerCoordinates.Longitude * Math.PI / 180;

            // Calculate new position
            double newLatRadians = Math.Asin(Math.Sin(centerLatRadians) * Math.Cos(randomDistance) +
                                    Math.Cos(centerLatRadians) * Math.Sin(randomDistance) * Math.Cos(randomAngle));
            double newLonRadians = centerLonRadians + Math.Atan2(Math.Sin(randomAngle) * Math.Sin(randomDistance) * Math.Cos(centerLatRadians),
                                    Math.Cos(randomDistance) - Math.Sin(centerLatRadians) * Math.Sin(newLatRadians));

            // Convert back to degrees
            double newLatitude = newLatRadians * 180 / Math.PI;
            double newLongitude = newLonRadians * 180 / Math.PI;

            return new Dictionary<string, object>
            {
                { "coordinates", new Dictionary<string, object>
                    {
                        { "latitude", newLatitude },
                        { "longitude", newLongitude }
                    }
                    
                },
                {
                    "region", new Dictionary<string, object>
                    {
                        {"city", "SOGAMOSO" },
                        {"country","Colombia" }
                    }
                },
                { "type", "Point" }
            };
        }

        public User GenerateDummyUser()
        {
            return new User
            {
                Id = Guid.NewGuid().ToString("N"),
                Name = GenerateRandomName(),
                Email = GenerateRandomEmail(),
                UserType = "care",
                Age = _random.Next(18, 70),
                Phone = GenerateRandomPhone(),
                Ubication = GenerateRandomUbication(),
                TypePetsCare = GenerateRandomPetTypes(),
                PhotoPerfil = $"https://randomuser.me/api/portraits/{(_random.Next(2) == 0 ? "men" : "women")}/{_random.Next(1, 100)}.jpg",
                CreationDate = Timestamp.GetCurrentTimestamp(),
                Verified = _random.Next(2) == 0,
                Status = "active"
            };
        }

        public string GenerateRandomName()
        {
            string[] firstNames = { "John", "Jane", "Mike", "Emily", "David", "Sarah" };
            string[] lastNames = { "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia" };
            return $"{firstNames[_random.Next(firstNames.Length)]} {lastNames[_random.Next(lastNames.Length)]}";
        }

        public string GenerateRandomEmail()
        {
            string[] domains = { "gmail.com", "yahoo.com", "hotmail.com", "outlook.com" };
            string name = GenerateRandomName().ToLower().Replace(" ", ".");
            return $"{name}@{domains[_random.Next(domains.Length)]}";
        }

        public string GenerateRandomPhone()
        {
            return $"+1{_random.Next(100, 999)}{_random.Next(100, 999)}{_random.Next(1000, 9999)}";
        }


        public List<string> GenerateRandomPetTypes()
        {
            string[] petTypes = { "dog", "cat", "bird", "fish", "rabbit" };
            int numTypes = _random.Next(1, 4);
            var selectedTypes = new HashSet<string>();
            while (selectedTypes.Count < numTypes)
            {
                selectedTypes.Add(petTypes[_random.Next(petTypes.Length)]);
            }
            return selectedTypes.ToList();
        }

        public async Task SaveUserToFirestore(User user)
        {
            var userDoc = _firestoreDb.Collection("users").Document(user.Id);

            await userDoc.SetAsync(user);
        }

        public async Task GenerateAndSaveRandomService(string userId)
        {
            var service = GenerateRandomService(userId);
            await SaveServiceToFirestore(service);
        }

        private Service GenerateRandomService(string userId)
        {
            return new Service
            {
                id = Guid.NewGuid().ToString("N"),
                active = _random.Next(2) == 0,
                idUser = userId,
                location = GenerateRandomUbication(),
                recoveryPets = _random.Next(2) == 0,
                serviceSelected = _random.Next(2) == 0 ? "GUARDERIA" : "PELUQUERIA",
                sizePets = GenerateRandomSizePets(),
                timeHavePets = GenerateRandomTimeHavePets(),
                typePetsCare = GenerateRandomTypePetsCare(),
                typeofServicesGrooming = GenerateRandomTypeofServicesGrooming(),
                valueService = GenerateRandomValueService()
            };
        }

    

        private List<string> GenerateRandomSizePets()
        {
            var sizes = new List<string> { "small", "medium", "large" };
            return sizes.GetRange(0, _random.Next(1, sizes.Count + 1));
        }

        private string GenerateRandomTimeHavePets()
        {
            var times = new List<string> { "less than 1 year", "1-3 years", "3-5 years", "more than 5 years" };
            return times[_random.Next(times.Count)];
        }

        private List<string> GenerateRandomTypePetsCare()
        {
            var types = new List<string> { "dog", "cat", "bird", "fish", "rabbit" };
            var selectedTypes = new List<string>();
            int numTypes = _random.Next(1, types.Count + 1);
            for (int i = 0; i < numTypes; i++)
            {
                var type = types[_random.Next(types.Count)];
                if (!selectedTypes.Contains(type))
                {
                    selectedTypes.Add(type);
                }
            }
            return selectedTypes;
        }

        private List<Dictionary<string, string>> GenerateRandomTypeofServicesGrooming()
        {
            var services = new List<Dictionary<string, string>>();
            var possibleServices = new List<string> { "bath", "haircut", "nail trimming", "teeth cleaning" };
            int numServices = _random.Next(1, possibleServices.Count + 1);
            for (int i = 0; i < numServices; i++)
            {
                services.Add(new Dictionary<string, string>
                {
                    { "service", possibleServices[_random.Next(possibleServices.Count)] },
                    { "price", $"{_random.Next(10, 101)}" }
                });
            }
            return services;
        }

        private string GenerateRandomValueService()
        {
            return $"{_random.Next(20, 201)}";
        }

        private async Task SaveServiceToFirestore(Service service)
        {
            var serviceDoc = _firestoreDb.Collection("services").Document(service.id);
            await serviceDoc.SetAsync(service);
        }



    }
}
