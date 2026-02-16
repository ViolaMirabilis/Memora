using Memora.Services;
using Memora.Core;
using Memora.Model;

namespace Memora.ViewModels;
/// <summary>
/// Not sure about the "!", but i'll leave it here for now.
/// </summary>
public class RevisionResultViewModel : ViewModel
{
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

    public RevisionResultViewModel(SessionService sessionService)
    {
        _sessionService = sessionService;
        TotalAnswers = SetTotalAnswers();
        StillLearning = SetStillLearning();
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
}
