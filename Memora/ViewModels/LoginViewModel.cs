using Memora.Core;
using Memora.Interfaces;

namespace Memora.ViewModels;

public class LoginViewModel : ViewModel
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

    public RelayCommand NavigateHomeCommand { get; set; }
    public RelayCommand NavigateRegisterCommand { get; set; }
    
    public LoginViewModel(INavigationService navService)
    {
        _navigation = navService;
        NavigateHomeCommand = new RelayCommand(o => { Navigation.NavigateTo<HomeViewModel>(); }, o => true);
        NavigateRegisterCommand = new RelayCommand(o => { Navigation.NavigateTo<RegisterViewModel>(); }, o => true);
    }
}
