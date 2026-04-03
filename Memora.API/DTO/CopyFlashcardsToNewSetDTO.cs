namespace SimpleAUTH.DTO
{
    public class CopyFlashcardsToNewSetDTO
    {
        public int SetId { get; set; }
        public List<FlashcardDTO> Flashcards { get; set; } = new List<FlashcardDTO>();
    }
}
