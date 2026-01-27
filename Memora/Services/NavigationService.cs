using Memora.Core;
using Memora.Interfaces;
namespace Memora.Services;

public class NavigationService : ObservableObject, INavigationService
{
    private readonly Func<Type, ViewModel> _viewModelFactory;       //Type is what view model we want to navigate to. It's set in App.xaml Accepts a method which takes Type as parameter and returns ViewModel
    private  ViewModel _currentView;            // a backing field of type ViewModel (which every ViewModel inherits from)
    public ViewModel CurrentView
    {
        get => _currentView;

        private set
        {
            _currentView = value;
            OnPropertyChanged();
        }   
    }

    public NavigationService(Func<Type, ViewModel> viewModelFactory)        // then we return a singleton instance of the view model
    {
        _viewModelFactory = viewModelFactory;
    }

    public void NavigateTo<TViewModel>() where TViewModel : ViewModel     // accepts classes that are typeof ViewModel
    {
        // A new view model is created here (from the Func delegate. Type is taken, ViewModel is returned)
        ViewModel viewModel = _viewModelFactory?.Invoke(typeof(TViewModel));        // so if we pass NavigateTo<HomeViewModel>, the TViewModel is casted to HomeViewModel.
        CurrentView = viewModel;        // the returned view model is assigned to the CurrentView variable
    }
}
