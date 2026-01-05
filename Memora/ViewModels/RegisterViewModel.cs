using Memora.Core;
using Memora.Interfaces;
using Memora.View;

namespace Memora.ViewModels
{
    public class RegisterViewModel : ViewModel
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

        public RelayCommand NavigateLoginCommand { get; set; }
        public RegisterViewModel(INavigationService navService)
        {
            _navigation = navService;
            NavigateLoginCommand = new RelayCommand(o => { Navigation.NavigateTo<LoginViewModel>(); }, o => true);
        }
    }
}
