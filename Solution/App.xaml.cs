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

            services.AddSingleton<IDataService, PassTestStats>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<ApiClient>();

            services.AddSingleton<MainViewModel>();
            services.AddSingleton<ScoreViewModel>();
            services.AddSingleton<LeaderboardViewModel>();
            services.AddSingleton<AccountViewModel>();
            services.AddSingleton<MultiplayerViewModel>();
            services.AddSingleton<HeaderViewModel>();
            services.AddSingleton<SendPrompt>();
            services.AddSingleton<ScoreViewModel>();
            services.AddSingleton<TypeTextViewModel>();
            services.AddSingleton<TestResultsViewModel>();

            services.AddSingleton<Func<Type, BaseViewModel>>(serviceProvider =>
                viewModelType => (BaseViewModel)serviceProvider.GetRequiredService(viewModelType));



            _serviceProvider = services.BuildServiceProvider();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            ApiClient apiClient = new ApiClient();
            IDataService passTestStats = new PassTestStats();
            SendPrompt sendPrompt = new SendPrompt();
            INavigationService navigationService = _serviceProvider.GetRequiredService<INavigationService>();
            ScoreViewModel viewModel = new ScoreViewModel(navigationService, sendPrompt, passTestStats, apiClient);
            await viewModel.InitializeAsync1();
            navigationService.NavigateTo<ScoreViewModel>();
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
            base.OnStartup(e);
        }
    }
}
