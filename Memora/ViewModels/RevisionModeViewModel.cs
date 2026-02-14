using Memora.Core;
using Memora.Interfaces;
using Memora.Model;
using Memora.Services;
using System.Collections.ObjectModel;

namespace Memora.ViewModels;

public class RevisionModeViewModel : ViewModel
{

    private INavigationService _navigation;
    public INavigationService Navigation
    {
        get => _navigation;
        set
        {
            _navigation = value;
            OnPropertyChanged();
        }
    }
    private readonly SessionService _sessionService;         // holds the session context to pass around other study modes

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
        set { _currentIndex = value;
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
        set { _isRevisionFinished = value; OnPropertyChanged(); }
    }


    public List<Flashcard> Flashcards { get; set; } = new List<Flashcard>();

    public RelayCommand GoNextCommand { get; set; }
    public RelayCommand GoPreviousCommand { get; set; }

    public RevisionModeViewModel(INavigationService navService, SessionService session)
    {
        Navigation = navService;
        _sessionService = session;
        LoadFlashcardsToCollection();
        SetFlashcard();
        _flashcardsCount = SetFlashcardsCount(Flashcards);          // sets flashcards count on view model creation
        GoNextCommand = new RelayCommand(_ => GoNext(), _ => CanGoNext());
        GoPreviousCommand = new RelayCommand(_ => GoPrevious(), _ => CanGoPrevious());
    }

    /// <summary>
    /// Adds flashcards to the observable collection during the initialisation of the VM
    /// </summary>
    private void LoadFlashcardsToCollection()
    {
        var sessionContext = GetDataFromSession();
        if (sessionContext != null)
        {
            foreach (var flashcard in sessionContext)
            {
                Flashcards.Add(flashcard);
            }
        }
        return;
        
    }

    #region Command Logic
    private void GoNext()
    {
        CurrentIndex++;
        if (_currentIndex == Flashcards.Count)      // if the button is pressed at the max capacity (i.e. 15/15), CurrentIndex is switched to 0 and a bool is toggled.
        {
            CurrentIndex = 0;   
            ToggleRevisionFinished();
        }
        SetFlashcard();
    }
    private bool CanGoNext()
    {
        return CurrentIndex <= FlashcardsCount;
    }
    private void GoPrevious()
    {
        CurrentIndex--;
        SetFlashcard();
    }
    private bool CanGoPrevious()
    {
        return CurrentIndex >= 1;
    }
    #endregion


    #region Helpers
    /// <summary>
    /// Sets the first flashcard to be visible.
    /// </summary>
    private void SetFlashcard()
    {
        Front = Flashcards[CurrentIndex].Front;
        Back = Flashcards[CurrentIndex].Back;
    }

    private void ToggleRevisionFinished()
    {
        IsRevisionFinished = true;
    }

    private int SetFlashcardsCount(List<Flashcard> flashcards)
    {
        return flashcards.Count;
    }

    // Assigns session data to the singleton SessionService.
    private List<Flashcard> GetDataFromSession()
    {
        return _sessionService.CurrentSession.FlashcardsCollection!;
    }


    #endregion
}
