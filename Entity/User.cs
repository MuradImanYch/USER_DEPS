using System.Text.Json.Serialization;

namespace USER_DEPS.Entity
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; } = string.Empty;

        public int PersonalId { get; set; }

        [JsonIgnore]
        public Personal Personal { get; set; } = null!;
    }
}
