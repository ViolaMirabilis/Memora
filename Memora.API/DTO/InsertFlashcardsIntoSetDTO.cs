using SimpleAUTH.DTO;

namespace MemoraAPI.DTO
{
    public class InsertFlashcardsIntoSetDTO
    {
        public List<FlashcardDTO> Flashcards { get; set; } = new List<FlashcardDTO>(); 
    }
}
