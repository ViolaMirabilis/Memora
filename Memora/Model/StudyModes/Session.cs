namespace Memora.Model.StudyModes;

public class Session
{
    public FlashcardSet? FlashcardSet { get; init; }
    public List<Flashcard>? FlashcardsCollection { get; set; }
    public Result? Result { get; set; }
}
