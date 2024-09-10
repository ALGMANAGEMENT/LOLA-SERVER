using Google.Cloud.Firestore;

namespace LOLA_SERVER.API.Models.PetService
{
    [FirestoreData]
    public class TypeService
    {
        [FirestoreProperty]
        public string Id { get; set; }

        [FirestoreProperty]
        public string value { get; set; }
    }


}
