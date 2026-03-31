using Microsoft.EntityFrameworkCore;
using SimpleAUTH.Data;
using SimpleAUTH.DTO;
using SimpleAUTH.Interfaces;
using SimpleAUTH.Models;
using System.Reflection.Metadata.Ecma335;

namespace SimpleAUTH.Services
{
    public class FlashcardService : IFlashcardService
    {
        private readonly FlashcardsDbContext _dbContext;

        public FlashcardService(FlashcardsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Flashcard> CreateFlashcard(int userId, Flashcard flashcard)       // not async yet
        {
            flashcard.UserId = userId;     // we get to the userID via FlashcardSet, because a flashcard CANNOT exist without FlashcardSet
            flashcard.Id = 0;              // setting it to 0 here, but it's changed later on by DB. Prevents client injection  
            var savedFlashcard = await _dbContext.Flashcards.AddAsync(flashcard);
            _dbContext.SaveChanges();

            return savedFlashcard.Entity;
        }

        public async Task<string?> DeleteFlashcard(int userId, int id)
        {
            Flashcard savedFlashcard = await _dbContext.Flashcards.FirstOrDefaultAsync(u => u.Id == id && u.FlashcardSet.UserId == userId);

            if (savedFlashcard == null)
                return null;

            _dbContext.Flashcards.Remove(savedFlashcard);
            _dbContext.SaveChanges();

            return $"Successfully deleted a flashcard!";
        }

        public async Task<List<Flashcard>> GetAllUserFlashcards(int userId)
        {
            return await _dbContext.Flashcards.Where(u => u.FlashcardSet.UserId == userId).ToListAsync();
        }
        /// <summary>
        ///  returns flashcards that match user's ID and flashcards that match the setId (Set name: English Vocab, and all the flashcards belonging to this set are returned)
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="setId"></param>
        /// <returns></returns>
        public async Task<List<Flashcard>> GetFlashcardsFromSet(int userId, int setId)
        {
            return await _dbContext.Flashcards.Where(u => u.UserId == userId && u.FlashcardSetId == setId).ToListAsync();
        }

        public async Task<List<Flashcard>> GetSharedFlashcardsFromSet(int setId)
        {
            // checks if the set is shared
            var sharedSet = await _dbContext.FlashcardSets.FirstOrDefaultAsync(f => f.Id == setId && f.IsSharing == true);
            if (sharedSet == null)
                return new List<Flashcard>();
            // if set is shared, return all the flashcards
            return await _dbContext.Flashcards.Where(u => u.FlashcardSetId == setId).ToListAsync();
        }

        public async Task<Flashcard> GetFlashcardById(int userId, int id)
        {
            Flashcard savedFashcard = _dbContext.Flashcards.FirstOrDefault(u => u.Id == id && u.FlashcardSet.UserId == userId);
            return savedFashcard;
        }

        public async Task<Flashcard> UpdateFlashcard(int userId, int id, Flashcard updatedFlashcard)
        {
            Flashcard savedFlashcard = await _dbContext.Flashcards.FirstOrDefaultAsync(u => u.Id == id && u.FlashcardSet.UserId == userId);

            if (savedFlashcard == null)
                throw new Exception("Flashcard not found");

            // sets new front and back. Doesn't check anything else for now
            savedFlashcard.Front = updatedFlashcard.Front;
            savedFlashcard.Back = updatedFlashcard.Back;

            //_dbContext.Entry(savedFlashcard).CurrentValues.SetValues(updatedFlashcard);
            _dbContext.SaveChanges();
            
            return savedFlashcard;
        }

        // gets flashcard from a passed in List<Flashcard> and creates new objects of it, assigning it to another user.
        public async Task CopyFlashcardsFromSharedSet(int userId, CopyFlashcardsToNewSet dto)
        {
            // list of FlashcardDTOs
            var flashcards = dto.Flashcards;
            var setId = dto.SetId;
            // gets all the flashcards where the setId matches
            // foreach flashcard that exists in the passed list, create a new object 
            // from DTOs, new flashcards are made
            var FlashcardsToClone = flashcards.Select(f => new Flashcard
            {
                // not including Flashcard Id in order to autoincrement
                // setId is taken from the passed in setId
                UserId = userId,
                Front = f.Front,
                Back = f.Back,
                FlashcardSetId = setId
            }).ToList();

            // adds copied flashcards to the db
            await _dbContext.Flashcards.AddRangeAsync(FlashcardsToClone);
            await _dbContext.SaveChangesAsync();
        }
    }
}
