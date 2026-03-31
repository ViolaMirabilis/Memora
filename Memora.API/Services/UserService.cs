using Microsoft.EntityFrameworkCore;
using SimpleAUTH.Data;
using SimpleAUTH.Interfaces;
using SimpleAUTH.Models;

namespace SimpleAUTH.Services
{
    public class UserService : IUserService
    {
        private readonly FlashcardsDbContext _dbContext;
        public UserService(FlashcardsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<User> CreateUser(User user)
        {
            var savedUser = await _dbContext.Users.AddAsync(user);
            _dbContext.SaveChanges();

            return savedUser.Entity;
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<User> GetUserById(int id)
        {
            User? savedUser = await _dbContext.Users.FindAsync(id);
            return savedUser;
        }

        public async Task<User> UpdateUser(int id, User updatedUser)
        {
            User savedUser = await _dbContext.Users.FindAsync(id);
            if (savedUser == null)
                return null;

            _dbContext.Entry(savedUser).CurrentValues.SetValues(updatedUser);
            _dbContext.SaveChanges();

            return savedUser;
        }
    }
}
