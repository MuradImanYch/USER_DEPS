using System.Text.Json.Serialization;

namespace USER_DEPS.Entity
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; } = string.Empty;
         
        [JsonIgnore]
        public string PasswordHash { get; set; } = string.Empty;

        public int? PersonalId { get; set; }

        [JsonIgnore]
        public Personal Personal { get; set; } = null!;

        public string? Token { get; set; }
    }
}