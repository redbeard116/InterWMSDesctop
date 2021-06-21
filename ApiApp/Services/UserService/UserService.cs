using ApiApp.Models;
using ApiApp.Providers.RequestProvider;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiApp.Services.UserService
{
    public class UserService : IUserService
    {
        #region Fields
        private readonly ILogger<UserService> _logger;
        private readonly IRequestProvider _requestProvider;
        #endregion

        #region Constructor
        public UserService(ILogger<UserService> logger,
                           IRequestProvider requestProvider)
        {
            _logger = logger;
            _requestProvider = requestProvider;
        }
        #endregion

        #region IUserService
        public async Task<int?> AddUser(User user)
        {
            _logger.LogInformation($"AddUser");

            try
            {
                var result = await _requestProvider.PostJson<User>("api/users", user.ToJson());
                return result.Id;
            }
            catch (System.Exception ex)
            {
                _logger.LogInformation($"AddUser error {ex.Message}");
                return null;
            }
        }

        public async Task<bool> DeleteUser(int id)
        {
            _logger.LogInformation($"DeleteUser {id}");
            try
            {
                await _requestProvider.Delete($"api/users/{id}");
                return true;
            }
            catch (System.Exception ex)
            {
                _logger.LogInformation($"DeleteUser error {ex.Message}");
                return false;
            }
        }

        public async Task<User> EditUser(User user)
        {
            _logger.LogInformation($"EditUser {user.Id}");
            try
            {
                return await _requestProvider.PutJson<User>($"api/users/edit/{user.Id}", user.ToJson());
            }
            catch (System.Exception ex)
            {
                _logger.LogInformation($"EditUser error {ex.Message}");
                return null;
            }
        }

        public async Task<User> GetUser(int id)
        {
            _logger.LogInformation($"GetUser {id}");
            try
            {
                return await _requestProvider.GetJson<User>($"/api/users/{id}");
            }
            catch (System.Exception ex)
            {
                _logger.LogInformation($"GetUser error {ex.Message}");
                return null;
            }
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            _logger.LogInformation($"GetUsers");
            try
            {
                var result = await _requestProvider.GetJsonString("/api/users");
                var json = JsonConvert.DeserializeObject<IEnumerable<User>>(result);
                return json;
            }
            catch (System.Exception ex)
            {
                _logger.LogInformation($"GetUsers error {ex.Message}");
                return null;
            }
        }

        public async Task<User> GetCurrentUser()
        {
            _logger.LogInformation($"GetCurrentUser");
            try
            {
                return await _requestProvider.GetJson<User>($"/api/users/currentuser");
            }
            catch (System.Exception ex)
            {
                _logger.LogInformation($"GetUsers error {ex.Message}");
                return null;
            }
        }
        #endregion
    }
}
