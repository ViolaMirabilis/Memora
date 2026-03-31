using Microsoft.AspNetCore.Razor.TagHelpers;
using SimpleAUTH.Data;
using SimpleAUTH.Models;

namespace SimpleAUTH.Interfaces
{
    public interface IFlashcardFolderService
    {
        Task<List<FlashcardFolder>> GetAllFlashcardFolders(int userId);     // overkill, lol.
        Task<FlashcardFolder> GetFlashcardFolderById(int userId, int id);
        Task<FlashcardFolder> CreateFlashcardFolder(int userId, FlashcardFolder flashcardFolder);
        Task<FlashcardFolder> UpdateFlashcardFolder(int userId, int id, FlashcardFolder updatedFlashcardFolder);
        Task<string?> DeleteFlashcardFolder(int userId, int id);
    }
}
