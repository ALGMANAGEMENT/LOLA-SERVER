using Google.Cloud.Firestore;

namespace LOLA_SERVER.API.Models.Users
{
    [FirestoreData]
    public class Service
    {
        [FirestoreProperty]
        public string Id { get; set; }

        [FirestoreProperty]
        public bool Active { get; set; }

        [FirestoreProperty]
        public string IdUser { get; set; }

        [FirestoreProperty]
        public Dictionary<string, object> Location { get; set; }

        [FirestoreProperty]
        public bool RecoveryPets { get; set; }

        [FirestoreProperty]
        public string ServiceSelected { get; set; }

        [FirestoreProperty]
        public List<string> SizePets { get; set; }

        [FirestoreProperty]
        public string TimeHavePets { get; set; }

        [FirestoreProperty]
        public List<string> TypePetsCare { get; set; }

        [FirestoreProperty]
        public List<Dictionary<string, string>> TypeofServicesGrooming { get; set; }

        [FirestoreProperty]
        public string ValueService { get; set; }
    }
}
