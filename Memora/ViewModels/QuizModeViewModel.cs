using Memora.Core;
using Memora.Interfaces;
using Memora.Model;
using Memora.Model.StudyModes;
using Memora.Services;
using System.Collections.ObjectModel;

namespace Memora.ViewModels.StudyModes;

public class QuizModeViewModel : ViewModel
{
    private readonly List<Flashcard> _flashcards = new List<Flashcard>();
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
    private readonly QuizModeService _quizService;

    #region Properties
    // Bindable observable collection with question + 4 answers (1 correct and 3 wrong)
    public ICollection<QuizAnswer> QuizAnswers { get; set; }
    public int TotalFlashcards => _quizService.TotalQuestions;
    #endregion

    #region Commands
    RelayCommand SubmitQuizCommand { get; set; }
    #endregion

    #region Constructor
    public QuizModeViewModel(INavigationService navService, SessionService session, QuizModeService quiz)
    {
        Navigation = navService;
        _sessionService = session;
        _quizService = quiz;
        InitialiseFlashcardsFromSession();
        InitialiseQuizMode();
        SubmitQuizCommand = new RelayCommand(_ => GoToReusltPage(), _ => CanGoToResultPage());
    }
    #endregion

    #region Command Logic


    #endregion
    void InitialiseFlashcardsFromSession()
    {
        // gets flashcard set from the session
        var session = _sessionService.CurrentSession.FlashcardsCollection!;
        // initialises the flashcards for the quiz mode
        _quizService.InitialiseFlashcards(session);
    }
    void InitialiseQuizAnswers()
    {
        QuizAnswers = _quizService.GetQuizAnswers();
    }

    void InitialiseQuizMode()
    {
        _quizService.InitialiseQuizData();
        InitialiseQuizAnswers();
    }
    // placeholder
    public void GoToReusltPage()
    {
        // to do
    }

    // placeholder
    public bool CanGoToResultPage()
    {
        return true;
    }
}
