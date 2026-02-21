using Memora.Model;
namespace Memora.Services;

/// <summary>
/// Logic for the Revision Mode as a separate service.
/// </summary>
public class RevisionModeService
{
    private List<Flashcard> _flashcards = new List<Flashcard>();

    #region Properties
    public int TotalFlashcards { get; private set; }
    // Current index is the current index of the flashcard
    public int CurrentIndex { get; private set; }
    // false by default
    public bool IsRevisionFinished { get; private set; }

    #endregion

    #region Logic Methods
    // Sets the front and the back of the flashcard to be visible.
    public void InitialiseFlashcards(ICollection<Flashcard> flashcards)
    {
        _flashcards = flashcards.ToList();
        TotalFlashcards = flashcards.ToList().Count;
        CurrentIndex = 0;
    }
    public ICollection<Flashcard> GetFlashcardsCollection()
    {
        return _flashcards;
    }
    public Flashcard GetCurrentFlashcard()
    {
        return _flashcards[CurrentIndex];
    }
    // Togles the bool to true once revision is finished.
    public void OnRevisionFinished()
    {
        IsRevisionFinished = true;
    }
    public void GoNext()
    {
        if (CurrentIndex >= TotalFlashcards - 1)      // if the button is pressed at the max capacity (i.e. 15/15), CurrentIndex is switched to 0 and a bool is toggled.
        {
            OnRevisionFinished();
            return;
        }

        CurrentIndex++;
    }
    public bool CanGoNext(int currentIndex)
    {
        return CurrentIndex <= TotalFlashcards;
    }
    public void GoPrevious(int currentIndex)
    {
        if (CanGoPrevious(currentIndex))
            CurrentIndex--;
    }
    public bool CanGoPrevious(int currentIndex)
    {
        return CurrentIndex > 0;
    }
    
    #endregion

}
