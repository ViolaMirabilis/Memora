using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using SimpleAUTH.Data;
using SimpleAUTH.DTO;
using SimpleAUTH.Helpers;
using SimpleAUTH.Interfaces;
using SimpleAUTH.Models;

namespace SimpleAUTH.Services
{
    public class FlashcardSetService :  IFlashcardSetService
    {
        private readonly FlashcardsDbContext _dbContext;
        private readonly SharingCode sharingCode = new SharingCode();

        public FlashcardSetService(FlashcardsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<FlashcardSet> CreateFlashcardSet(int userId, FlashcardSet flashcardSet)
        {
            flashcardSet.UserId = userId;       // FlashcardSet userId is set to the one taken from JWT token
            flashcardSet.Id = 0;                // setting it to 0 here, but it's changed later on by DB. Prevents client injection
            var savedFlashcardSet = await _dbContext.FlashcardSets.AddAsync(flashcardSet);
            await _dbContext.SaveChangesAsync();

            return savedFlashcardSet.Entity;
        }

        public async Task<string?> DeleteFlashcardSet(int userId, int id)
        {
            // returns a flashcard set if the ID and USER ID have matches.
            FlashcardSet savedFlashcardSet = await _dbContext.FlashcardSets.FirstOrDefaultAsync(u => u.Id == id && u.UserId == userId);

            if (savedFlashcardSet == null)
                return null;

            _dbContext.FlashcardSets.Remove(savedFlashcardSet);
            _dbContext.SaveChanges();

            return $"Successfully deleted a flashcard set with id: {id}";
        }

        public async Task<List<FlashcardSet>> GetAllFlashcardSets(int userId)
        {
            // returns a list of flashcard sets where the user ID matches.
            // including the navigation properties here
            return await _dbContext.FlashcardSets.Where(u => u.UserId == userId)
                .Include(f => f.Flashcards)
                .Include(f => f.Folder)
                .ToListAsync();
        }

        public async Task<FlashcardSet> GetFlashcardSetById(int userId, int id)
        {
            FlashcardSet savedFlashcardSet = await _dbContext.FlashcardSets.FirstOrDefaultAsync(u => u.Id == id && u.UserId == userId);
            if (savedFlashcardSet == null)
                return null;

            return savedFlashcardSet;
        }

        public async Task<FlashcardSet> UpdateFlashcardSet(int userId, int id, FlashcardSet updatedFlashcardSet)
        {
            FlashcardSet savedFlashcardSet = await _dbContext.FlashcardSets
                .Include(u => u.Flashcards)         // loads all the flashcards related to teh flashcard set. Otherwise, the collection would be empty.
                .FirstOrDefaultAsync(u => u.Id == id && u.UserId == userId);

            if (savedFlashcardSet == null)
                throw new Exception("Flashcard set not found");

            // temporarily not updating name + folderId. Might move it to a separate endpoint
            //savedFlashcardSet.Name = updatedFlashcardSet.Name;
            //savedFlashcardSet.FolderId = updatedFlashcardSet.FolderId;
            savedFlashcardSet.Flashcards = updatedFlashcardSet.Flashcards;

            //_dbContext.Entry(savedFlashcardSet).CurrentValues.SetValues(updatedFlashcardSet);
            await _dbContext.SaveChangesAsync();

            return savedFlashcardSet;
        }

        public async Task<bool> UpdateFlashcardSetName(int userId, int id, UpdatedNameFlashcardSetDTO updatedSet)
        {
            // gets existing set
            // if flaschardSetID == updatedSetID
            var existingFlashcard = await _dbContext.FlashcardSets.FirstOrDefaultAsync(u => u.Id == id && u.UserId == userId);
            // returns false if person does not exist
            if (existingFlashcard == null)
                return false;

            // updates its name with a new one
            existingFlashcard.Name = updatedSet.Name;

            // saves to the db
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ShareFlashcardSet(int userId, int id)
        {
            
            // gets the FlashcardSet by ID
            var existingFlashcardSet = await _dbContext.FlashcardSets.FirstOrDefaultAsync(u => u.Id == id && u.UserId == userId);
            if (existingFlashcardSet == null)
                return false;
            else if (existingFlashcardSet.IsSharing == true)
                return false;

            // If set isn't null OR if the user is already sharing, early exit.
            // generates a code. Make it a recursive function later on.
            var code = sharingCode.GenerateCode();


            existingFlashcardSet.SharingCode = code;
            existingFlashcardSet.IsSharing = true;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<string> GetSharingCodeFlashcardSet(int userId, int id)
        {
            var existingFlashcardSet = await _dbContext.FlashcardSets.FirstOrDefaultAsync(u => u.Id == id && u.UserId == userId);

            if (existingFlashcardSet == null)
                return null;

            return existingFlashcardSet.SharingCode;
        }

        public async Task<FlashcardSet> GetFlashcardSetBySharingCode(string code)
        {
            FlashcardSet existingFlashcardSet = await _dbContext.FlashcardSets.FirstOrDefaultAsync(u => u.SharingCode == code);

            if (existingFlashcardSet == null)
                return null;

            return existingFlashcardSet;
        }

        public async Task<FlashcardSet> GetLastFlashcardSet(int userId)
        {
            // return the LAST ADDED flashcard set where the user ID matches. Sort by flashcard set ID
            return await _dbContext.FlashcardSets.Where(u => u.UserId == userId).OrderBy(f => f.Id).LastOrDefaultAsync();   // verifies original user ID with provided one
        }

        /// <summary>
        /// Updates the "LastStudied" property on the flashcard set.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> UpdateLastStudied(int userId, int id)
        {
            var existingFlashcardSet = await _dbContext.FlashcardSets.FirstOrDefaultAsync(u => u.Id == id && u.UserId == userId);

            if (existingFlashcardSet == null)
                return false;

            existingFlashcardSet.LastStudied = DateTime.Now;
            await _dbContext.SaveChangesAsync();
            return true;
        }


        /// <summary>
        /// Retrieves the 5 most recently studied flashcards sets for a given user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<FlashcardSet>> GetRecentlyStudiedFlashcardSets(int userId)
        {
            return await _dbContext.FlashcardSets.Where(u => u.UserId == userId)
                .Include(f => f.Flashcards)
                .Include(f => f.Folder)
                .OrderByDescending(f => f.LastStudied)
                .Take(5)
                .ToListAsync();
        }

    }

}
