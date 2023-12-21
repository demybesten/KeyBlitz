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
            services.AddSingleton<LoginRegisterViewModel>();
            services.AddSingleton<Func<Type, BaseViewModel>>(serviceProvider =>
                viewModelType => (BaseViewModel)serviceProvider.GetRequiredService(viewModelType));



            _serviceProvider = services.BuildServiceProvider();
        }

        protected async override void OnStartup(StartupEventArgs e)
        {
            INavigationService navigationService = _serviceProvider.GetRequiredService<INavigationService>();

            // Check if we can fetch user info
            ApiClient api = new ApiClient();
            ApiResponse response = await api.GetUserInfo();

            // No valid request, go to login
            if (!response.Success)
            {
                navigationService.NavigateTo<LoginRegisterViewModel>();
            }
            else
            {
                navigationService.NavigateTo<ScoreViewModel>();
            }


            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
            base.OnStartup(e);
        }
    }
}
