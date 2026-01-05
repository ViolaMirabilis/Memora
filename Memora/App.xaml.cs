using Memora.ViewModels;
using Memora.Services;
using Memora.Core;
using NavigationService = Memora.Services.NavigationService;        // to resolve namespace issues
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using Memora.Interfaces;
using System.Windows.Navigation;
using Memora.Authentication;

namespace Memora
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;

        /// <summary>
        /// Starting point of the application.
        /// </summary>
        public App()
        {
            IServiceCollection services = new ServiceCollection();      // Stores services in the DI container
            services.AddSingleton<MainWindow>(serviceProvider => new MainWindow // Only one instance of MainWindow is created throughout the entire application (Singleton).
            {
                DataContext = serviceProvider.GetRequiredService<MainViewModel>()       // assigning a view model to the MainWindow view. It gets it from the DI container. It runs whenever the MainWindow is requested.
            });

            //token and auth
            services.AddSingleton<ITokenStore, TokenStore>();       // singleton, the token is stored ONCE
            services.AddTransient<ApiClientMessageHandler>();       
            services.AddHttpClient("ApiClient", client =>
            {
                client.BaseAddress = new Uri("https://localhost:1234/");
            });

            //Api
            services.AddTransient<FlashcardApiService>();
            

            // view model services
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<RegisterViewModel>();
            services.AddSingleton<LoginViewModel>();
            services.AddSingleton<HomeViewModel>();
            services.AddSingleton<MyFlashcardsViewModel>();
            services.AddSingleton<MyFoldersViewModel>();
            services.AddSingleton<MyProfileViewModel>();
            services.AddSingleton<SettingsViewModel>();

            // navigation service
            services.AddSingleton<INavigationService, NavigationService>();
            // 
            services.AddSingleton<Func<Type, ViewModel>>(serviceProvider => viewModelType => (ViewModel)serviceProvider.GetRequiredService(viewModelType));     // a factory delegate (do some reading)

            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetService<MainWindow>();     // Gets an instance of the service from the DI container
            mainWindow!.Show();
            base.OnStartup(e);
        }
        
    }

}
