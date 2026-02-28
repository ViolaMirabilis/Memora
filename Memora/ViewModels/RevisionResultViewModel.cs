using Memora.Services;
using Memora.Core;
using Memora.Model;
using Memora.Interfaces;

namespace Memora.ViewModels;
/// <summary>
/// Not sure about the "!", but i'll leave it here for now.
/// </summary>
public class RevisionResultViewModel : ViewModel
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

    private int _totalAnswers = 0;
    public int TotalAnswers
    {
        get => _totalAnswers;
        set { _totalAnswers = value; OnPropertyChanged(); }
    }

    private int _stillLearning = 0;
    public int StillLearning
    {
        get => _stillLearning;
        set { _stillLearning = value; OnPropertyChanged(); }
    }
    public RelayCommand NavigateRevisionModeCommand { get; set; }       // repeated...
    public RelayCommand ShuffleCommand { get; set; }
    public RevisionResultViewModel(INavigationService navService, SessionService sessionService)
    {
        _navigation = navService;
        _sessionService = sessionService;
        TotalAnswers = SetTotalAnswers();
        StillLearning = SetStillLearning();

        NavigateRevisionModeCommand = new RelayCommand(o => { Navigation.NavigateTo<RevisionModeViewModel>(); }, o => true);
        ShuffleCommand = new RelayCommand(o => Shuffle(), o => true);
    }

    private int SetTotalAnswers()
    {
        return _sessionService.CurrentSession.Result!.TotalAnswers;
    }
    private int SetStillLearning()
    {
        var result = _sessionService.CurrentSession.Result.StillLearning.Count;
        if (result <= 0)
            return 0;

        return _sessionService.CurrentSession.Result.StillLearning.Count;
    }
    private void Shuffle()
    {
        var shuffledList = _sessionService.CurrentSession.FlashcardsCollection!.Shuffle();
        _sessionService.CurrentSession.FlashcardsCollection = shuffledList.ToList();
        Navigation.NavigateTo<RevisionModeViewModel>();
    }
}
