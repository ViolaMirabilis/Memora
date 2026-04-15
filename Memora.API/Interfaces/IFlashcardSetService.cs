using SimpleAUTH.DTO;
using SimpleAUTH.Models;

namespace SimpleAUTH.Interfaces
{
    public interface IFlashcardSetService
    {
        Task<List<FlashcardSet>> GetAllFlashcardSets(int userId);
        Task<FlashcardSet> GetFlashcardSetById(int userId, int id);
        Task<FlashcardSet> CreateFlashcardSet(int userId, FlashcardSet flashcardSet);
        Task<FlashcardSet> UpdateFlashcardSet(int userId, int id, FlashcardSet updatedFlashcardSet);
        Task<string?> DeleteFlashcardSet(int userId, int id);
        // Updates the name of the FlashcardSet
        Task<bool> UpdateFlashcardSetName(int userId, int id, UpdatedNameFlashcardSetDTO dto);
        Task<bool> ShareFlashcardSet(int userId, int id);
        Task<string> GetSharingCodeFlashcardSet(int userId, int id);
        Task<FlashcardSet> GetFlashcardSetBySharingCode( string code);
        Task<FlashcardSet> GetLastFlashcardSet(int userId);
        Task<bool> UpdateLastStudied(int userId, int id);
    }
}
