using Google.Cloud.Firestore;

namespace LOLA_SERVER.API.Models.Users
{
    [FirestoreData]
    public class Service
    {
        [FirestoreProperty]
        public string id { get; set; }

        [FirestoreProperty]
        public bool active { get; set; }

        [FirestoreProperty]
        public string idUser { get; set; }

        [FirestoreProperty]
        public Dictionary<string, object> location { get; set; }

        [FirestoreProperty]
        public bool recoveryPets { get; set; }

        [FirestoreProperty]
        public string serviceSelected { get; set; }

        [FirestoreProperty]
        public List<string> sizePets { get; set; }

        [FirestoreProperty]
        public string timeHavePets { get; set; }

        [FirestoreProperty]
        public List<string> typePetsCare { get; set; }

        [FirestoreProperty]
        public List<Dictionary<string, string>> typeofServicesGrooming { get; set; }

        [FirestoreProperty]
        public string valueService { get; set; }
    }
}
