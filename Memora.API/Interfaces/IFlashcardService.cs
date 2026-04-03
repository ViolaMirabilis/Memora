using SimpleAUTH.DTO;
using SimpleAUTH.Models;

namespace SimpleAUTH.Interfaces
{
    public interface IFlashcardService
    {
        Task<List<Flashcard>> GetAllUserFlashcards(int userId);     // overkill, lol.
        Task<List<Flashcard>> GetFlashcardsFromSet(int userId, int setId);
        Task<Flashcard> GetFlashcardById(int userId, int id);
        Task<Flashcard> CreateFlashcard(int userId, Flashcard flashcard);
        Task<Flashcard> UpdateFlashcard(int userId, int id, Flashcard updatedFlashcard);
        Task<string?> DeleteFlashcard(int userId, int id);
        Task<List<Flashcard>> GetSharedFlashcardsFromSet(int setId);
        Task CopyFlashcardsFromSharedSet(int userId, CopyFlashcardsToNewSetDTO flashcards);
    }
}
