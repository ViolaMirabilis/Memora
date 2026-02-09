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
    public string Front { get; set; } = "Hello";
    public string Back { get; set; } = "World";
    public List<Flashcard> Flashcards { get; set; } = new List<Flashcard>();

    public RevisionModeViewModel(INavigationService navService, SessionService session)
    {
        Navigation = navService;
        _sessionService = session;
        LoadFlashcardsToCollection();

        Front = Flashcards[0].Front;
        Back = Flashcards[0].Back;
    }

    private void LoadFlashcardsToCollection()
    {
        var sessionContext = _sessionService.CurrentSession.FlashcardsCollection;
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
