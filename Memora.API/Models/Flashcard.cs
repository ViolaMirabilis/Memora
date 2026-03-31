using System.Security.Cryptography.X509Certificates;

namespace SimpleAUTH.Models
{
    /// <summary>
    /// Flashcard must live in a set, because the user cannot make just one flashcard alone.
    /// </summary>
    public class Flashcard
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FlashcardSetId { get; set; }         // FK
        public FlashcardSet FlashcardSet { get; set; } = null!;     // cannot be null. Used for navigation
        public string Front { get; set; } = string.Empty;
        public string Back { get; set; } = string.Empty;
    }
}
