using ApiApp.Models;
using ApiApp.Providers.RequestProvider;
using System.Security.Claims;

namespace ApiApp.Providers.UserProvider
{
    public interface IUserProvider
    {
        void UpdateUser(ClaimsIdentity user);
        ClaimsIdentity GetUser();
        IRequestProvider GetRequestProvider();
        string Name { get; }
        UserRole Role { get; }
    }
}
