using ApiApp.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ApiApp.Services.AuthService
{
    public interface IAuthService
    {
        Task<ClaimsIdentity> AuthServer(string login, string password);
        Task<User> CurrentUserInfo(int id);
    }
}
