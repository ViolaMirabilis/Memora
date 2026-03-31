using SimpleAUTH.Models;

namespace SimpleAUTH.DTO
{
    public class FlashcardDTO
    {
        public int Id { get; set; }
        public int FlashcardSetId { get; set; }
        public string Front { get; set; } = string.Empty;
        public string Back { get; set; } = string.Empty;
    }
}
