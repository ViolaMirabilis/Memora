namespace SimpleAUTH.DTO
{
    public class CopyFlashcardsToNewSet
    {
        public int SetId { get; set; }
        public List<FlashcardDTO> Flashcards { get; set; } = new List<FlashcardDTO>();
    }
}
