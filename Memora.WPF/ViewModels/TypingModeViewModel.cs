using Memora.Core;
using Memora.Interfaces;

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

    public TypingModeViewModel(INavigationService navService)
    {
        _navigation = navService;
    }
}
