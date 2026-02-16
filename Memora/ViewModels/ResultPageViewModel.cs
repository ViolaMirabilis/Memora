using Memora.Core;
using Memora.Model;
using Memora.Services;

namespace Memora.ViewModels
{
    public class ResultPageViewModel : ViewModel
    {
        private readonly SessionService _sessionService;

        private int _totalAnswers = 0;
        public int TotalAnswers
        {
            get => _totalAnswers;
            set { _totalAnswers = value; OnPropertyChanged(); }
        }

        public ResultPageViewModel(SessionService sessionService)
        {
            _sessionService = sessionService;
            TotalAnswers = _sessionService.CurrentSession.Result!.TotalAnswers;
        }
    }
}
