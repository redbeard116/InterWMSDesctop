using Newtonsoft.Json;

namespace ApiApp.Models
{
    public class UserIdenity
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("role")]
        public string Role { get; set; }
    }
}
