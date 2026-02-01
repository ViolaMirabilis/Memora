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

    /// <summary>
    /// Overload method. The NavigateTo method now needs a specified View Model and a Action<TViewModel> delegate
    /// Which is ANY METHOD that takes TViewModel as parameter and returns void
    /// In this case, the TViewModel is already set when we call the NavigateTo method
    /// So any further method used on it, will correspond to this view model
    /// Action<TviewModel> is basically:
    /// public void (TViewModel vm) {// some body}
    /// So the vm => ... used in the call is just a shorthand for that
    /// (MyFlashcardSetDataViewModel(vm) => {_ = vm.LoadAsync(set.Id);}
    /// </summary>
    /// <typeparam name="TViewModel"></typeparam>
    /// <param name="initialise"></param>
    public void NavigateTo<TViewModel>(Action<TViewModel> initialise) where TViewModel : ViewModel
    {
        var viewModel = (TViewModel)_viewModelFactory?.Invoke(typeof(TViewModel));
        initialise(viewModel);      // setting the instance of the desired view model
        CurrentView = viewModel;
    }
}
