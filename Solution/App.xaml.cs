using System;
using Solution.ViewModels;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Solution.Services;
using Solution.Views;
using ScoreViewModel = Solution.ViewModels.ScoreViewModel;

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
            services.AddSingleton<MultiplayerViewModel>();
            services.AddSingleton<SendPrompt>();
            services.AddSingleton<HeaderViewModel>();
            services.AddSingleton<TypeTextViewModel>();
            services.AddSingleton<TestResultsViewModel>();
            services.AddSingleton<INavigationService, NavigationService>();

            services.AddScoped<ScoreViewModel>();

            services.AddSingleton<Func<Type, BaseViewModel>>(serviceProvider =>
                viewModelType => (BaseViewModel)serviceProvider.GetRequiredService(viewModelType));



            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            INavigationService navigationService = _serviceProvider.GetRequiredService<INavigationService>();
            navigationService.NavigateTo<ScoreViewModel>();
           var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
           mainWindow.Show();
           base.OnStartup(e);
        }
    }
}
