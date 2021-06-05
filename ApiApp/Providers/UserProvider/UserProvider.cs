using ApiApp.Models;
using ApiApp.Providers.RequestProvider;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Linq;
using System;

namespace ApiApp.Providers.UserProvider
{
    public class UserProvider : IUserProvider
    {
        #region Fields
        private readonly ILogger<UserProvider> _logger;
        private ClaimsIdentity _currentUser;
        private readonly IRequestProvider _requestProvider;
        #endregion

        #region Constructor
        public UserProvider(ILogger<UserProvider> logger,
                            IRequestProvider requestProvider)
        {
            _logger = logger;
            _requestProvider = requestProvider;
        }

        public IRequestProvider GetRequestProvider()
        {
            return _requestProvider;
        }
        #endregion

        #region IUserProvider
        public ClaimsIdentity GetUser()
        {
            return _currentUser;
        }

        public void UpdateUser(ClaimsIdentity user)
        {
            _logger.LogInformation("Update user");
            _currentUser = user;
            _requestProvider.UpdateUser(user);
        }

        public string Name => _currentUser.Name;

        public UserRole Role
        {
            get
            {
                var userIdentity = (ClaimsIdentity)_currentUser;
                var claims = userIdentity.Claims;
                var role = claims.Where(c => c.Type == ClaimTypes.Role).FirstOrDefault().Value;
                return (UserRole)Enum.Parse(typeof(UserRole), role);
            }
        }
        #endregion
    }
}
