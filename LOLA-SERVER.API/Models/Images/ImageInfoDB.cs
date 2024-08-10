using Google.Cloud.Firestore;

namespace LOLA_SERVER.API.Models.Images
{
    [FirestoreData]
    public class ImageInfoDB
    {
        [FirestoreProperty]
        public string Url { get; set; }
        [FirestoreProperty]
        public string UserId { get; set; }
        [FirestoreProperty]
        public string OriginalFileName { get; set; }
        [FirestoreProperty]
        public string FilePath { get; set; }
        [FirestoreProperty]
        public string NameHash { get; set; }
        [FirestoreProperty]
        public DateTime UploadDate { get; set; }

        [FirestoreProperty]
        public DateTime CreatedDate { get; set; }
    }
}
