using Memora.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;

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
            services.AddSingleton<RegisterViewModel>();
            services.AddSingleton<LoginViewModel>();
            services.AddSingleton<HomeViewModel>();
            services.AddSingleton<MyFlashcardsViewModel>();
            services.AddSingleton<MyFoldersViewModel>();
            services.AddSingleton<MyProfileViewModel>();
            services.AddSingleton<SettingsViewModel>();

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
