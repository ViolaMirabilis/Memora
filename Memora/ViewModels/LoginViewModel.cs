using Memora.Core;
using Memora.Interfaces;
using Memora.Model;
using Memora.Services;
using System.Net.Http;
using System.Windows;

namespace Memora.ViewModels;
/// <summary>
/// Note to self: move this into a separate service
/// </summary>
public class LoginViewModel : ViewModel
{
    private readonly AuthApiService _authApi;

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

    private string _username;
    public string Username
    {
        get => _username;
        set { _username = value; OnPropertyChanged(); }
    }

    private string _password;
    public string Password
    {
        get => _password;
        set { _password = value; OnPropertyChanged(); }
    }


    public RelayCommand NavigateHomeCommand { get; set; }
    public RelayCommand NavigateRegisterCommand { get; set; }
    public RelayCommand LoginCommand { get; set; }// change to async later on
    
    public LoginViewModel(INavigationService navService, AuthApiService authService)
    {
        _authApi = authService;
        _navigation = navService;

        LoginCommand = new RelayCommand(async _ => { await LoginAsync(); },o=> true);
        NavigateHomeCommand = new RelayCommand(_ => { Navigation.NavigateTo<HomeViewModel>();}, _ => true);
        //NavigateHomeCommand = new RelayCommand(o => { Navigation.NavigateTo<HomeViewModel>(); }, o => true);
        NavigateRegisterCommand = new RelayCommand(_ => { Navigation.NavigateTo<RegisterViewModel>(); }, _ => true);
    }

    private async Task LoginAsync()
    {
        try
        {
            var loginRequest = new LoginRequest(Username, Password);     // creates a new LoginRequest object

            await _authApi.LoginAsync(loginRequest);
            Navigation.NavigateTo<HomeViewModel>();         // executes if the login is successfull
        }
        catch (HttpRequestException ex)
        {
            MessageBox.Show(ex.Message);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

}
