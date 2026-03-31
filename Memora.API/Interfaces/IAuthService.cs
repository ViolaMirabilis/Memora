using SimpleAUTH.DTO;
using SimpleAUTH.Models;

namespace SimpleAUTH.Interfaces
{
    public interface IAuthService
    {
        // Async methods to register and login
        Task<User?> RegisterAsync(RegisterRequest request);
        Task<string?> LoginAsync(LoginRequest request);
    }
}
