namespace Memora.Model;

public class Session
{
    public FlashcardSet? FlashcardSet { get; init; }
    public List<Flashcard>? FlashcardsCollection { get; init; }
    public Result? Result { get; set; }
}
