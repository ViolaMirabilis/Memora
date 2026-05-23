using Memora.Core;
using Memora.Interfaces;
using Memora.Model;
using Memora.Services;
using System.Collections.ObjectModel;

namespace Memora.ViewModels;

public class TypingModeViewModel : ViewModel
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

    private readonly SessionService _sessionService;
    
    // holds the flashcards from the current session
    public ICollection<Flashcard> Flashcards { get; set; } = new ObservableCollection<Flashcard>();

    // holds the current answer that the user typed into the textbox.
    // yet to decide whether the user can input all answers at once, then get them checked
    // or if the user should first answer and only then be able to proceed to the next question
    private string _currentAnswer;
    public string CurrentAnswer
    {
        get => _currentAnswer;
        set
        {
            _currentAnswer = value;
            OnPropertyChanged();
        }
    }

    public TypingModeViewModel(INavigationService navService, SessionService session)
    {
        _navigation = navService;
        _sessionService = session;

        InitialiseFlashcards();
    }

    // initialises the data from the current session
    public void InitialiseFlashcards()
    {
        // Gets session info from the Singleton Session service
        var session = _sessionService.CurrentSession.FlashcardsCollection;

        if (session != null)
        {
            Flashcards = session;
        }
    }
}
