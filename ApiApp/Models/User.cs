using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ApiApp.Models
{
    public class User : JsonBase
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public UserRole Role { get; set; }

        public override bool HasValue => GetHasValue();

        private bool GetHasValue()
        {
            return !string.IsNullOrWhiteSpace(FirstName) && 
                   !string.IsNullOrWhiteSpace(SecondName) &&
                   !string.IsNullOrWhiteSpace(Login) &&
                   !string.IsNullOrWhiteSpace(Password);
        }
    }

    public enum UserRole
    {
        Admin,
        User,
        Security,
        Manager
    }
}
