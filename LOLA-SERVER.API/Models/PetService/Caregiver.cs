namespace LOLA_SERVER.API.Models.PetService
{
    public class Caregiver
    {
        public string id { get; set; }
        public string name { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string userType { get; set; }
        public object ubication { get; set; }
        public List<string> typePetsCare { get; set; }
        public string photoPerfil { get; set; }

        public static Caregiver FromDictionary(string id, Dictionary<string, object> data)
        {
            return new Caregiver
            {
                id = id,
                name = data.TryGetValue("name", out var name) ? name.ToString() : null,
                lastName = data.TryGetValue("lastName", out var lastName) ? lastName.ToString() : null,
                email = data.TryGetValue("email", out var email) ? email.ToString() : null,
                phone = data.TryGetValue("phone", out var phone) ? phone.ToString() : null,
                userType = data.TryGetValue("userType", out var userType) ? userType.ToString() : null,
                ubication = data.TryGetValue("ubication", out var ubication) ? ubication : null,
                typePetsCare = data.TryGetValue("typePetsCare", out var typePetsCare) ? typePetsCare as List<string> : null,
                photoPerfil = data.TryGetValue("photoPerfil", out var photoPerfil) ? photoPerfil.ToString() : null
            };
        }
    }
}