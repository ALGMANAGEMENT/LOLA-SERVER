using Google.Cloud.Firestore;

namespace LOLA_SERVER.API.Models.Notifications
{
    [FirestoreData]
    public class NotificationDBData
    {
        [FirestoreProperty]
        public string IdNotification { get; set; }

        [FirestoreProperty]
        public string Message { get; set; }

        [FirestoreProperty]
        public Timestamp CreationDate { get; set; }

        [FirestoreProperty]
        public bool Read { get; set; }

        [FirestoreProperty]
        public string Status { get; set; }

        [FirestoreProperty]
        public string Type { get; set; }

        [FirestoreProperty]
        public string TypeNotification { get; set; }

        [FirestoreProperty]
        public string User { get; set; }

        [FirestoreProperty]
        public List<string> Topics { get; set; }
    }
}
