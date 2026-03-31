using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SimpleAUTH.Data;
using SimpleAUTH.DTO;
using SimpleAUTH.Interfaces;
using SimpleAUTH.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SimpleAUTH.Services
{
    /// <summary>
    /// Passing in the dbContext as parameter, so the service can access the database.
    /// </summary>
    /// <param name="dbContext"></param>
    public class AuthService(FlashcardsDbContext dbContext, IConfiguration configuration) : IAuthService
    {
        public async Task<string?> LoginAsync(LoginRequest request)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == request.Username.ToLower());

            if (user == null) return null;

            // We create a new PasswordHasher instance. We pass in the user object and check its HASHED password with the request password.
            // If it fails, we return null.
            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
            {
                return null;
            }

            user.LastLoginAt = DateTime.UtcNow;
            await dbContext.SaveChangesAsync();
            // we return the token if login is successful
            return CreateToken(user);       
        }

        /// <summary>
        /// Reguster async method. We pass in a UserDTO object with username and password.
        /// We check if the user exists in database, if not - return null.
        /// If it doesn't exist, create a new user with a hashed password and save it to the database.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<User?> RegisterAsync(RegisterRequest request)
        {
            // Checking if the user already exists

            if (await dbContext.Users.AnyAsync(u => u.Username.ToLower() == request.Username.ToLower()))
                return null;        // we just return null if the user exists. In the controller we will handle this case.

            var user = new User();
            // creaters a hashed password from the received UserDTO
            var hashedPassword = new PasswordHasher<User>()
                .HashPassword(user, request.Password);

            user.Username = request.Username;
            user.PasswordHash = hashedPassword;
            user.Nickname = request.Nickname;
            user.CreatedAt = DateTime.UtcNow;

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();

            return user;
        }

        private string CreateToken(User user)
        {

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),           // our ID is stored in the token
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!));     // refers to appsettings.json file
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescription = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("Appsettings:Issuer"),
                audience: configuration.GetValue<string>("Appsettings:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),        // 1 day
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescription);
        }
    }
}
