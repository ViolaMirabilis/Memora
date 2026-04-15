namespace SimpleAUTH.Models
{
    /// <summary>
    /// Consists of multiple Flashcard objects.
    /// </summary>
    public class FlashcardSet
    {
        public required int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// The set may or may not belong to a folder.
        /// </summary>
        public int? FolderId { get; set; }
        public string FolderName { get; } = string.Empty;
        public string? SharingCode { get; set; }
        public bool IsSharing { get; set; } = false;
        // MinVal is set by default
        public DateTime LastStudied { get; set; } = DateTime.MinValue;
        //Navigation only
        public User User { get; set; } = null!;
        public FlashcardFolder? Folder { get; set; }

        public ICollection<Flashcard> Flashcards { get; set; } = new List<Flashcard>();
    }
}
