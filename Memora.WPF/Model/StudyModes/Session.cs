namespace Memora.Model.StudyModes;

public class Session
{
    public FlashcardSet? FlashcardSet { get; private set; }
    public List<Flashcard>? FlashcardsCollection { get; private set; }
    public Result? Result { get; private set; }

    public void SetFlashcardSet(FlashcardSet set)
    {
        FlashcardSet = set;
    }

    public void SetFlashcardCollection(List<Flashcard> sets)
    {
        FlashcardsCollection = sets;
    }

    public void SetResult(Result result)
    {
        Result = result;
    }
}
