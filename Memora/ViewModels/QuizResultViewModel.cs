using Memora.Core;
using Memora.Interfaces;
using Memora.Services;

namespace Memora.ViewModels;

public class QuizResultViewModel : ViewModel
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

    private int _totalQuestions = 0;
    public int TotalQuestions
    {
        get => _totalQuestions;
        set { _totalQuestions = value; OnPropertyChanged(); }
    }

    private int _correctAnswers = 0;
    public int CorrectAnswers
    {
        get => _correctAnswers;
        set { _correctAnswers = value; OnPropertyChanged(); }
    }

    private int _incorrectAnswers = 0;
    public int IncorrectAnswers
    {
        get => _incorrectAnswers;
        set { _incorrectAnswers = value; OnPropertyChanged(); }
    }

    public RelayCommand NavigateQuizModeCommand { get; set; }       // repeated...
    public RelayCommand ShuffleCommand { get; set; }
    public QuizResultViewModel(INavigationService navService, SessionService sessionService)
    {
        _navigation = navService;
        _sessionService = sessionService;
        TotalQuestions = SetTotalQuestions();
        CorrectAnswers = SetCorrectAnswers();
        IncorrectAnswers = SetIncorrectAnswers();

        NavigateQuizModeCommand = new RelayCommand(o => { Navigation.NavigateTo<QuizModeViewModel>(); }, o => true);
        ShuffleCommand = new RelayCommand(o => Shuffle(), o => true);
    }

    private int SetTotalQuestions()
    {
        return _sessionService.CurrentSession.Result!.TotalAnswers;
    }
    
    private int SetCorrectAnswers()
    {
        return _sessionService.CurrentSession.Result!.CorrectAnswers;
    }
    private int SetIncorrectAnswers()
    {
        return _sessionService.CurrentSession.Result!.IncorrectAnswers;
    }
    private void Shuffle()
    {
        var shuffledList = _sessionService.CurrentSession.FlashcardsCollection!.Shuffle();
        _sessionService.CurrentSession.FlashcardsCollection = shuffledList.ToList();
        Navigation.NavigateTo<QuizModeViewModel>();
    }
}
