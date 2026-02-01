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
            // @see https://www.milanjovanovic.tech/blog/the-right-way-to-use-httpclient-in-dotnet?utm_source=YouTube&utm_medium=social&utm_campaign=26.01.2025
            // and this one: https://www.milanjovanovic.tech/blog/extending-httpclient-with-delegating-handlers-in-aspnetcore?utm_source=chatgpt.com
            // //@see https://learn.microsoft.com/en-us/dotnet/core/extensions/httpclient-factory
            services.AddSingleton<ITokenStorage, TokenStorage>();       // singleton, the token is stored ONCE
            services.AddTransient<ApiClientMessageHandler>();       
            services.AddHttpClient("ApiClient", client =>           // this is a named client. It'll be used throughout the entire program.
            {
                client.BaseAddress = new Uri("https://localhost:7153/");
            })
            .AddHttpMessageHandler<ApiClientMessageHandler>();      // adding the handler here. Once the JWT token is set, it will be added to every request made with this client.

            //Api
            services.AddTransient<FlashcardApiService>();
            services.AddTransient<AuthApiService>();
            services.AddTransient<FlashcardSetApiService>();
            

            // view model services
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<RegisterViewModel>();
            services.AddSingleton<LoginViewModel>();
            services.AddSingleton<HomeViewModel>();
            services.AddSingleton<MyFlashcardSetDisplayViewModel>();
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
