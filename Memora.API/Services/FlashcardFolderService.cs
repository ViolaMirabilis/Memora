using Microsoft.EntityFrameworkCore;
using SimpleAUTH.Data;
using SimpleAUTH.Interfaces;
using SimpleAUTH.Models;

namespace SimpleAUTH.Services
{
    public class FlashcardFolderService : IFlashcardFolderService
    {
        private readonly FlashcardsDbContext _dbContext;
        public FlashcardFolderService(FlashcardsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<FlashcardFolder> CreateFlashcardFolder(int userId, FlashcardFolder flashcardFolder)
        {
            flashcardFolder.UserId = userId;
            var savedFlashcardFolder = await _dbContext.FlashcardFolders.AddAsync(flashcardFolder);
            _dbContext.SaveChanges();

            return savedFlashcardFolder.Entity;
        }

        public async Task<string?> DeleteFlashcardFolder(int userId, int id)
        {
            FlashcardFolder savedFlashcardFolder = await _dbContext.FlashcardFolders.FirstOrDefaultAsync(u => u.Id == id && u.UserId == userId);
            if (savedFlashcardFolder == null)
                return null;

            _dbContext.FlashcardFolders.Remove(savedFlashcardFolder);
            _dbContext.SaveChanges();

            return $"Successfully deleted a flashcard folder with id: {id}";
        }

        public async Task<List<FlashcardFolder>> GetAllFlashcardFolders(int userId)
        {
            return await _dbContext.FlashcardFolders.Where(u => u.UserId == userId).ToListAsync();
        }

        public async Task<FlashcardFolder> GetFlashcardFolderById(int userId, int id)
        {
            FlashcardFolder savedFlashcardFolder = await _dbContext.FlashcardFolders.FirstOrDefaultAsync(u => u.Id == id && u.UserId == userId);
            return savedFlashcardFolder;
        }

        public async Task<FlashcardFolder> UpdateFlashcardFolder(int userId, int id, FlashcardFolder updatedFlashcardFolder)
        {
            FlashcardFolder savedFlashcardFolder = await _dbContext.FlashcardFolders.FirstOrDefaultAsync(u => u.Id == id && u.UserId == userId);

            if (savedFlashcardFolder == null)
                return null;

            _dbContext.Entry(savedFlashcardFolder).CurrentValues.SetValues(updatedFlashcardFolder);
            _dbContext.SaveChanges();

            return savedFlashcardFolder;
        }
    }
}
