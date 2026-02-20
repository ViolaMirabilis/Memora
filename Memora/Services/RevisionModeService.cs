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
    public void InitialiseFlashcards(IEnumerable<Flashcard> flashcards)
    {
        _flashcards = flashcards.ToList();
        TotalFlashcards = flashcards.ToList().Count;
        CurrentIndex = 0;
    }
    public List<Flashcard> GetFlashcardsList()
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
        CurrentIndex++;
        if (CurrentIndex == TotalFlashcards)      // if the button is pressed at the max capacity (i.e. 15/15), CurrentIndex is switched to 0 and a bool is toggled.
        {
            CurrentIndex = 0;
            OnRevisionFinished();           // Once the revision is done, this method is called.
        }
        //SetFlashcard();
    }
    public bool CanGoNext()
    {
        return CurrentIndex <= TotalFlashcards;
    }
    public void GoPrevious()
    {
        CurrentIndex--;
    }
    public bool CanGoPrevious()
    {
        return CurrentIndex >= 1;
    }
    
    #endregion

}
