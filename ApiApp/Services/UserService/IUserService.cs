using ApiApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiApp.Services.UserService
{
    public interface IUserService
    {
        Task<int?> AddUser(User user);
        Task<bool> DeleteUser(int id);
        Task<User> EditUser(User user);
        Task<User> GetUser(int id);
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetCurrentUser();
    }
}
