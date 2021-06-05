using ApiApp.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApiApp.Services.AuthService
{
    public class AuthService : IAuthService
    {
        #region Fields
        private readonly ILogger<AuthService> _logger;
        private readonly HttpClient _client;
        #endregion

        #region Constructor
        public AuthService(ILogger<AuthService> logger,
                           HttpClient client)
        {
            _logger = logger;
            _client = client;
        }
        #endregion

        #region IAuthService
        public async Task<ClaimsIdentity> AuthServer(string login, string password)
        {
            _logger.LogInformation($"Auth server");

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
                throw new System.Exception("Логин или пароль не должен быть пустым");


            var body = GetParamString(login, password);
            var parametersString = new StringContent(body, Encoding.UTF8, "application/json");
            parametersString.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await _client.PostAsync(ConstructUrl(), parametersString);
            response.EnsureSuccessStatusCode();

            _logger.LogInformation($"Auth complited");

            var result = await response.Content.ReadAsStringAsync();

            _logger.LogTrace($"POST response. Body: {result}");

            var userIdenity =  JsonConvert.DeserializeObject<UserIdenity>(result);
            var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, userIdenity.Username),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, userIdenity.Role),
                    new Claim("Token",userIdenity.AccessToken)
                };
            ClaimsIdentity claimsIdentity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }

        public async Task<User> CurrentUserInfo(int id)
        {
            _logger.LogInformation($"CurrentUserInfo");
            var response = await _client.GetAsync($"api/user/currentuser{id}");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            _logger.LogTrace($"Get response. Body: {result}");

            var user = JsonConvert.DeserializeObject<User>(result);
            return user;
        }

        #endregion

        #region Private methods
        private string ConstructUrl()
        {
            return $"/api/auth";
        }

        private string GetParamString(string login, string password)
        {
            return $"{{\"login\":\"{login}\",\"password\":\"{password}\"}}";
        }
        #endregion
    }
}
