using Memora.Core;
using Memora.Interfaces;
using Memora.Model;
using Memora.Services;
using System.Collections.ObjectModel;

namespace Memora.ViewModels;

public class RevisionModeViewModel : StudyModeBase
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

    public RevisionModeViewModel(INavigationService navService, SessionService session) : base(session)
    {
        Navigation = navService;
        _sessionService = session;
        OnRevisionFinished += GoToResultPage;
        LoadFlashcardsToCollection();
        SetFlashcard();
        FlashcardsCount = SetFlashcardsCount(Flashcards);          // sets flashcards count on view model creation
        GoNextCommand = new RelayCommand(_ => GoNext(), _ => CanGoNext());
        GoPreviousCommand = new RelayCommand(_ => GoPrevious(), _ => CanGoPrevious());
    }

    private void GoToResultPage()
    {
        Navigation.NavigateTo<ResultPageViewModel>();
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

    
}
