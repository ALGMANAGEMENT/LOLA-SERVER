using Google.Cloud.Firestore;

namespace LOLA_SERVER.API.Models.PetService
{

    [FirestoreData]
    public class Booking
    {
        [FirestoreProperty("_idService")]
        public string IdService { get; set; }

        [FirestoreProperty("_idUserService")]
        public string IdUserService { get; set; }

        [FirestoreProperty("description")]
        public string Description { get; set; }

        [FirestoreProperty("infoService")]
        public InfoService InfoService { get; set; }

        [FirestoreProperty("location")]
        public Location Location { get; set; }

        [FirestoreProperty("petClean")]
        public List<string> PetClean { get; set; }

        [FirestoreProperty("petInfo")]
        public List<PetInfo> PetInfo { get; set; }

        [FirestoreProperty("requidePet")]
        public bool RequirePet { get; set; }

        [FirestoreProperty("timeEnd")]
        public string TimeEnd { get; set; }

        [FirestoreProperty("timeStart")]
        public string TimeStart { get; set; }

        [FirestoreProperty("typeService")]
        public string TypeService { get; set; }

        [FirestoreProperty("typeSolicitude")]
        public string TypeSolicitude { get; set; }

        [FirestoreProperty("weight")]
        public string Weight { get; set; }

        [FirestoreProperty("sizeAnsuers")]
        public int SizeAnswers { get; set; }

        [FirestoreProperty("stateService")]
        public string StateService { get; set; }

        [FirestoreProperty("urlCares")]
        public List<string> UrlCares { get; set; }

        [FirestoreProperty("userCreate")]
        public UserCreate UserCreate { get; set; }
    }

    [FirestoreData]
    public class InfoService
    {
        [FirestoreProperty("dateEnd")]
        public string DateEnd { get; set; }

        [FirestoreProperty("dateStart")]
        public string DateStart { get; set; }
    }

    [FirestoreData]
    public class Location
    {
        [FirestoreProperty("address")]
        public string Address { get; set; }

        [FirestoreProperty("description")]
        public string Description { get; set; }

        [FirestoreProperty("latitude")]
        public double Latitude { get; set; }

        [FirestoreProperty("longitude")]
        public double Longitude { get; set; }

        [FirestoreProperty("locationCity")]
        public string LocationCity { get; set; }
    }

    [FirestoreData]
    public class PetInfo
    {
        [FirestoreProperty("Unitweigth")]
        public string UnitWeight { get; set; }

        [FirestoreProperty("age")]
        public string Age { get; set; }

        [FirestoreProperty("description")]
        public string Description { get; set; }

        [FirestoreProperty("name")]
        public string Name { get; set; }

        [FirestoreProperty("race")]
        public string Race { get; set; }

        [FirestoreProperty("sex")]
        public string Sex { get; set; }

        [FirestoreProperty("sizes")]
        public Sizes Sizes { get; set; }

        [FirestoreProperty("specy")]
        public string Species { get; set; }

        [FirestoreProperty("typeFood")]
        public string TypeFood { get; set; }

        [FirestoreProperty("unitAge")]
        public string UnitAge { get; set; }

        [FirestoreProperty("urlImage")]
        public string UrlImage { get; set; }

        [FirestoreProperty("vaccination")]
        public string Vaccination { get; set; }

        [FirestoreProperty("weigth")]
        public string Weight { get; set; }
    }

    [FirestoreData]
    public class Sizes
    {
        [FirestoreProperty("chest")]
        public string Chest { get; set; }

        [FirestoreProperty("large")]
        public string Large { get; set; }

        [FirestoreProperty("neck")]
        public string Neck { get; set; }
    }

    [FirestoreData]
    public class UserCreate
    {
        [FirestoreProperty("lastname")]
        public string LastName { get; set; }

        [FirestoreProperty("name")]
        public string Name { get; set; }
    }
}
