using Google.Cloud.Firestore;

namespace LOLA_SERVER.API.Models.Users
{
    [FirestoreData]
    public class User
    {
        [FirestoreProperty]
        public string Id { get; set; }

        [FirestoreProperty]
        public string Name { get; set; }

        [FirestoreProperty]
        public string Email { get; set; }

        [FirestoreProperty]
        public string UserType { get; set; }

        [FirestoreProperty]
        public int Age { get; set; }

        [FirestoreProperty]
        public string Phone { get; set; }

        [FirestoreProperty]
        public Dictionary<string, object> Ubication { get; set; }

        [FirestoreProperty]
        public List<string> TypePetsCare { get; set; }

        [FirestoreProperty]
        public string PhotoPerfil { get; set; }

        [FirestoreProperty]
        public Timestamp CreationDate { get; set; }

        [FirestoreProperty]
        public bool Verified { get; set; }

        [FirestoreProperty]
        public string Status { get; set; }
    }
}
