using System;
using Solution.ViewModels;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Solution.Services;
using Solution.Views;

namespace Solution
{
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;

        public App()
        {
            IServiceCollection services = new ServiceCollection();
            
            services.AddSingleton<MainWindow>(serviceProvider => new MainWindow
            {
                DataContext = serviceProvider.GetRequiredService<MainViewModel>()
            });
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<ScoreViewModel>();
            services.AddSingleton<LeaderboardViewModel>();
            services.AddSingleton<AccountViewModel>();
            services.AddSingleton<KBViewModel>();
            services.AddSingleton<NewTestViewModel>();
            services.AddSingleton<HeaderViewModel>();
            services.AddSingleton<INavigationService, NavigationService>();

            services.AddSingleton<Func<Type, BaseViewModel>>(serviceProvider =>
                viewModelType => (BaseViewModel)serviceProvider.GetRequiredService(viewModelType));

            
            
            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
           var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
           mainWindow.Show();
           base.OnStartup(e);
        }
    }
}