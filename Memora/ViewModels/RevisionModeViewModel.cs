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
    private readonly RevisionModeService _revisionService;

    #region Properties
    public int TotalFlashcards => _revisionService.TotalFlashcards;
    public int CurrentIndex => _revisionService.CurrentIndex;

    public int CurrentIndexDisplay => CurrentIndex + 1;

    private bool _isRevisionFinished;
    public bool IsRevisionFinished
    {
        get => _isRevisionFinished;
        set
        {
            _isRevisionFinished = value;
            OnPropertyChanged();
        }
    }

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

    public ICollection<Flashcard> Flashcards { get; set; } = new ObservableCollection<Flashcard>();

    public RelayCommand GoNextCommand { get; set; }
    public RelayCommand GoPreviousCommand { get; set; }

    #endregion
    public RevisionModeViewModel(INavigationService navService, SessionService session, RevisionModeService revision)
    {
        Navigation = navService;
        _sessionService = session;
        _revisionService = revision;
        InitialiseRevisionMode();
        GoNextCommand = new RelayCommand(_ => GoNext(), _ => true);
        GoPreviousCommand = new RelayCommand(_ => GoPrevious(), _ => CurrentIndex > 0);
    }

    #region Commands
    public void GoNext()
    {
        _revisionService.GoNext();
        if (_revisionService.IsRevisionFinished)
        {
            GoToResultPage();
        }
        // Notifies the index that is displayed to the user
        OnPropertyChanged(nameof(CurrentIndex));
        OnPropertyChanged(nameof(CurrentIndexDisplay));
        // notifies the button to reevaluate the condition (can/can't be pressed)
        GoPreviousCommand.RaiseCanExecuteChanged();
        SetFlashcards();
    }

    public void GoPrevious()
    {
        _revisionService.GoPrevious(CurrentIndex);
        OnPropertyChanged(nameof(CurrentIndex));
        OnPropertyChanged(nameof(CurrentIndexDisplay));
        // notifies the button to reevaluate the condition (can/can't be pressed)
        GoPreviousCommand.RaiseCanExecuteChanged();
        SetFlashcards();
    }
    public bool CanGoPrevious()
    {
        return CurrentIndex > 0;
    }
    #endregion

    public void InitialiseRevisionMode()
    {
        InitialiseFlashcards();
        SetFlashcards();
    }
    public void InitialiseFlashcards()
    {
        // Gets session info from the Singleton Session service
        var session = _sessionService.CurrentSession.FlashcardsCollection!;
        // Initialises current index, total flashcards and the collection
        _revisionService.InitialiseFlashcards(session);
        // assigns the collection to VM's observable collection to be used in the view
        Flashcards = new ObservableCollection<Flashcard>(_revisionService.GetFlashcardsCollection());
    }

    public void SetFlashcards()
    {
        Front = _revisionService.GetCurrentFlashcard().Front;
        Back = _revisionService.GetCurrentFlashcard().Back;
    }



    private void GoToResultPage()
    {
        var result = new Result { TotalAnswers = TotalFlashcards };
        _sessionService.NewResult(result);      // testing purposes
        Navigation.NavigateTo<RevisionResultViewModel>();

    }


}
