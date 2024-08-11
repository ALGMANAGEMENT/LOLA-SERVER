namespace LOLA_SERVER.API.Models.PetService
{
    public class Caregiver
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string UserType { get; set; }
        public Dictionary<string, object> Ubication { get; set; }
        public List<string> TypePetsCare { get; set; }
        public string PhotoPerfil { get; set; }

        public static Caregiver FromDictionary(string id, Dictionary<string, object> data)
        {
            return new Caregiver
            {
                Id = id,
                Name = data.TryGetValue("Name", out var name) ? name.ToString() : null,
                Email = data.TryGetValue("Email", out var email) ? email.ToString() : null,
                Phone = data.TryGetValue("Phone", out var phone) ? phone.ToString() : null,
                UserType = data.TryGetValue("UserType", out var userType) ? userType.ToString() : null,
                Ubication = data.TryGetValue("Ubication", out var ubication) ? ubication as Dictionary<string, object> : null,
                TypePetsCare = data.TryGetValue("TypePetsCare", out var typePetsCare) ? typePetsCare as List<string> : null,
                PhotoPerfil = data.TryGetValue("PhotoPerfil", out var photoPerfil) ? photoPerfil.ToString() : null
            };
        }
    }
}
