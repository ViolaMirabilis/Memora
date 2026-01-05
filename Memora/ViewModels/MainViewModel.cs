using Memora.Core;
using Memora.Interfaces;
using Memora.Services;

namespace Memora.ViewModels
{
    public class MainViewModel : ViewModel
    {
        // This navigation is enough to be passed to any view model from which we want to navigate
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
        public RelayCommand NavigateMyFlashcardsCommand { get; set; }
        public RelayCommand NavigateMyFoldersCommand { get; set; }
        public RelayCommand NavigateMyProfileCommand { get; set; }
        public RelayCommand NavigateSettingsCommand { get; set; }

        public MainViewModel(INavigationService navService)
        {
            Navigation = navService;
            NavigateHomeCommand = new RelayCommand(o => { Navigation.NavigateTo<HomeViewModel>(); }, o => true);    // action<object> (sth that returns void), predicate<object>
            NavigateMyFlashcardsCommand = new RelayCommand(o => { Navigation.NavigateTo<MyFlashcardsViewModel>(); }, o => true);
            NavigateMyFoldersCommand = new RelayCommand(o => { Navigation.NavigateTo<MyFoldersViewModel>(); }, o => true);
            NavigateMyProfileCommand = new RelayCommand(o => { Navigation.NavigateTo<MyProfileViewModel>(); }, o => true);
            NavigateSettingsCommand = new RelayCommand(o => { Navigation.NavigateTo<SettingsViewModel>(); }, o => true);

            Navigation.NavigateTo<LoginViewModel>();
        }
    }
}
