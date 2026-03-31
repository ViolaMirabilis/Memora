namespace SimpleAUTH.Models
{
    /// <summary>
    /// Contains Flashcard Sets (that contain of individual flashcards)
    /// </summary>
    public class FlashcardFolder
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;

        // Navigation
        public User User { get; set; } = null!;

        public ICollection<FlashcardSet> FlashcardSets { get; set; } = new List<FlashcardSet>();
    }
}
