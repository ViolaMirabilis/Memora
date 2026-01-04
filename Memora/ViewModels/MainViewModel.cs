using Memora.Core;
using Memora.Interfaces;
using Memora.Services;

namespace Memora.ViewModels
{
    public class MainViewModel : ViewModel
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

        public RelayCommand NavigateHomeCommand { get; set; }       // neds to be public so it can be bound to XAML
        public MainViewModel(INavigationService navService)
        {
            Navigation = navService;
            NavigateHomeCommand = new RelayCommand(o => { Navigation.NavigateTo<HomeViewModel>(); }, o => true);    // action<object> (sth that returns void), predicate<object>
        }
    }
}
