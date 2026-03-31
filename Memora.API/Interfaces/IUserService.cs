using SimpleAUTH.Data;
using SimpleAUTH.Models;

namespace SimpleAUTH.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsers();
        Task<User> GetUserById(int id);
        Task<User> CreateUser(User user);
        Task<User> UpdateUser(int id, User updatedUser);
    }
}
