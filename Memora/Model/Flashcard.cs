namespace Memora.Model;

public class Flashcard
{
    public int Id { get; set; } = 0;
    public int FlashcardSetId { get; set; }
    public string Front { get; set; } = string.Empty;
    public string Back { get; set; } = string.Empty;
}
