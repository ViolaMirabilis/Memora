namespace SimpleAUTH.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Username { get; set; } = string.Empty;
        public string? Nickname { get; set; } = string.Empty;
        public string? PasswordHash { get; set; } = string.Empty;
        public string? Role { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
        public DateTime LastLoginAt { get; set; }

        public ICollection<FlashcardSet> FlashcardSets { get; set; } = new List<FlashcardSet>();
        public ICollection<FlashcardFolder> FlashcardFolders { get; set; } = new List<FlashcardFolder>();
    }
}
