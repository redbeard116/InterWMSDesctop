using Newtonsoft.Json;

namespace ApiApp.Models
{
    public class Counterparty : JsonBase
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int Account { get; set; }
        public int INN { get; set; }

        [JsonIgnore]
        public string FullName => $"{User.FirstName} {User.SecondName}";
    }
}