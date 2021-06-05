using ApiApp.Providers.UserProvider;
using ApiApp.Services.AuthService;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace InterWMSDesctop
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        #region Fields
        private readonly IAuthService _authService;
        private readonly IUserProvider _userProvider;
        #endregion

        #region Constructor
        public AuthStateProvider(IAuthService authService,
                                 IUserProvider userProvider)
        {
            _authService = authService;
            _userProvider = userProvider;
        }
        #endregion

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();
            try
            {
                var userInfo = _userProvider.GetUser();
                if (userInfo != null && userInfo.IsAuthenticated)
                {
                    var claims = new[] { new Claim(ClaimTypes.Name, userInfo.Name) }.Concat(userInfo.Claims.Select(c => new Claim(c.Type, c.Value)));
                    identity = new ClaimsIdentity(claims, "Server authentication");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Request failed:" + ex.ToString());
            }
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        public async Task Login(string login, string password)
        {
            var user = await _authService.AuthServer(login, password);
            _userProvider.UpdateUser(user);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public void LogOut()
        {
            _userProvider.UpdateUser(null);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}