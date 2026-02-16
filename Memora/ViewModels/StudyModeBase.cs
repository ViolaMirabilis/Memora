using Memora.Core;
using Memora.Interfaces;
using Memora.Model;
using Memora.Services;
using System.Collections.ObjectModel;

namespace Memora.ViewModels;

public abstract class StudyModeBase : ViewModel
{
    private readonly SessionService _sessionService;
    public event Action OnRevisionFinished;
    public List<Flashcard> Flashcards { get; set; } = new List<Flashcard>();     // contains flashcards that are fetched from the API ("the original flashcards")

    #region Properties
    private string _front;
    public string Front
    {
        get { return _front; }
        set { _front = value; OnPropertyChanged(); }
    }

    private string _back;

    public string Back
    {
        get { return _back; }
        set { _back = value; OnPropertyChanged(); }
    }

    private int _flashcardsCount;
    public int FlashcardsCount
    {
        get => _flashcardsCount;
        set { _flashcardsCount = value; OnPropertyChanged(); }
    }

    private int _currentIndex = 0;
    public int CurrentIndex
    {
        get => _currentIndex;
        set
        {
            _currentIndex = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(CurrentIndexDisplay));     // nameof instead of "CurrentIndexDisplay", to avoid hard-coded values (even tho i've got plenty XD)
            GoPreviousCommand.RaiseCanExecuteChanged();         // these two raise an event, which re-checks if the command can be executed.
            GoNextCommand.RaiseCanExecuteChanged();
        }
    }

    public int CurrentIndexDisplay => CurrentIndex + 1;     // always adds 1 to current index for dispalying purposes

    private bool _isRevisionFinished;       // starts as false

    public bool IsRevisionFinished
    {
        get => _isRevisionFinished;
        set { _isRevisionFinished = value;
            OnPropertyChanged();
            OnRevisionFinished?.Invoke();           // Once revision is finished, it invokes the event, which calls the derived class.
        }
    }
    #endregion

    #region Commands
    public RelayCommand GoNextCommand { get; set; }
    public RelayCommand GoPreviousCommand { get; set; }
    #endregion

    #region Constructor
    public StudyModeBase(SessionService sessionService)
    {
        _sessionService = sessionService;

        GoNextCommand = new RelayCommand(_ => GoNext(), _ => CanGoNext());
        GoPreviousCommand = new RelayCommand(_ => GoPrevious(), _ => CanGoPrevious());
    }
    #endregion

    #region Command Logic
    public virtual void GoNext()
    {
        CurrentIndex++;
        if (CurrentIndex == Flashcards.Count)      // if the button is pressed at the max capacity (i.e. 15/15), CurrentIndex is switched to 0 and a bool is toggled.
        {
            CurrentIndex = 0;
            ToggleRevisionFinished();
        }
        SetFlashcard();
    }
    /// <summary>
    /// One the max count is reached, the user can still "GoNext", for further logic (i.e. bool change, result screen, etc)
    /// </summary>
    /// <returns></returns>
    public virtual bool CanGoNext()
    {
        return CurrentIndex <= FlashcardsCount;
    }
    public virtual void GoPrevious()
    {
        CurrentIndex--;
        SetFlashcard();
    }
    public virtual bool CanGoPrevious()
    {
        return CurrentIndex >= 1;
    }
    #endregion

    #region Helpers
    /// <summary>
    /// Sets the first flashcard to be visible.
    /// </summary>
    public virtual void SetFlashcard()
    {
        Front = Flashcards[CurrentIndex].Front;
        Back = Flashcards[CurrentIndex].Back;
    }

    public virtual void ToggleRevisionFinished()
    {
        IsRevisionFinished = true;
    }

    public virtual int SetFlashcardsCount(List<Flashcard> flashcards)
    {
        return flashcards.Count;
    }

    // Assigns session data to the singleton SessionService.
    public virtual List<Flashcard> GetDataFromSession()
    {
        return _sessionService.CurrentSession.FlashcardsCollection!;
    }
    #endregion
}
