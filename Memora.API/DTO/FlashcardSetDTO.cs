using SimpleAUTH.Models;

namespace SimpleAUTH.DTO
{
    public class FlashcardSetDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int? FolderId { get; set; }
        public string? FolderName { get; set; }
        public bool IsSharing { get; set; } = false;
        public string SharingCode { get; set; } = string.Empty;
        public DateTime LastStudied { get; set; } = DateTime.MinValue;
    }
}
