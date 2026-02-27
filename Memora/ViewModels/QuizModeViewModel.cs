using Memora.Core;
using Memora.Interfaces;
using Memora.Model;
using Memora.Model.StudyModes;
using Memora.Services;
using System.Windows;
using System.Windows.Controls;

namespace Memora.ViewModels;

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
    // a returned QuizAnswer object with already assigned "selected answer" property
    public QuizAnswer SelectedAnswer { get; set; }
    public int TotalFlashcards => _quizService.TotalQuestions;
    #endregion

    #region Commands
    // A submit button to submit the answers and go to the result page, where the user can see their score and which questions they got right/wrong
    public RelayCommand CheckAnswersCommand { get; set; }
    // A command for each answer button, which sets the selected answer for each question
    public RelayCommand SelectAnswerCommand { get; set; }
    #endregion

    #region Constructor
    public QuizModeViewModel(INavigationService navService, SessionService session, QuizModeService quiz)
    {
        Navigation = navService;
        _sessionService = session;
        _quizService = quiz;
        InitialiseFlashcardsFromSession();
        InitialiseQuizMode();
        // Reference: https://stackoverflow.com/questions/58214948/on-button-click-i-want-to-send-the-button-text-to-viewmodel-mvvm
        //SelectAnswerCommand = new RelayCommand(obj => SelectedAnswer = obj.ToString(), _ => true);
        SelectAnswerCommand = new RelayCommand(obj => AssignSelectedAnswer(obj), _ => true);
        CheckAnswersCommand = new RelayCommand(_ => CheckResults(), _ => CanCheckResults());
    }
    #endregion

    #region Command Logic
    // PLACEHOLDER
    private void CheckResults()
    {
        int correctAnswersCount = 0;
        foreach (var quizAnswer in QuizAnswers)
        {
            if (quizAnswer.SelectedAnswer == quizAnswer.CorrectAnswer)
            {
                correctAnswersCount++;
            }
        }

        MessageBox.Show($"Correct answers: {correctAnswersCount}\nIncorrect answers: {TotalFlashcards - correctAnswersCount}");
    }
    private bool CanCheckResults()
    {
        return QuizAnswers.Any(q => !string.IsNullOrEmpty(q.SelectedAnswer));

    }
    // Assigns the selected answer to its QuizAnswer object
    // Ivoked after pressing on the radio button
    public void AssignSelectedAnswer(object obj)
    {
        if (obj is RadioButton rb && rb.Tag is QuizAnswer quizAnswer)
        {
            // assigns the content of the RB to the selected answer property of the QuizAnswer object
            quizAnswer.SelectedAnswer = rb.Content.ToString() ?? string.Empty;
            //MessageBox.Show($"Selected answeR: {quizAnswer.SelectedAnswer.ToString()}");
        }
        // Recheks the command condition
        CheckAnswersCommand.RaiseCanExecuteChanged();
    }
    // placeholder
    public void GoToResultPage()
    {
        // to do
    }

    // placeholder
    public bool CanGoToResultPage()
    {
        return true;
    }

    #endregion
    void InitialiseFlashcardsFromSession()
    {
        // gets flashcard set from the session
        var session = _sessionService.CurrentSession.FlashcardsCollection!;
        // initialises the flashcards for the quiz mode
        _quizService.InitialiseFlashcards(session);
    }
    // Assigns questions + answers to the collection that is bound to the view
    void InitialiseQuizAnswers()
    {
        QuizAnswers = _quizService.GetQuizAnswers();
    }

    void InitialiseQuizMode()
    {
        _quizService.InitialiseQuizData();
        InitialiseQuizAnswers();
    }
    
}
