using System;
using Solution.ViewModels;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Solution.Services;
using System.Reflection;
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
            services.AddSingleton<TestResultsViewModel>();

            services.AddSingleton<MultiplayerResultsViewModel>();
            services.AddSingleton<WebserverService>();





            services.AddSingleton<Func<Type, BaseViewModel>>(serviceProvider =>
                viewModelType => (BaseViewModel)serviceProvider.GetRequiredService(viewModelType));



            _serviceProvider = services.BuildServiceProvider();
        }

        protected async override void OnStartup(StartupEventArgs e)
        {
            // Get required services
            INavigationService navigationService = _serviceProvider.GetRequiredService<INavigationService>();
            ApiClient api = _serviceProvider.GetRequiredService<ApiClient>();
            MainWindow mainWindow = _serviceProvider.GetRequiredService<MainWindow>();

            // Check if we can fetch user info
            ApiResponse response = await api.GetUserInfo();

            // Determine the ViewModel to navigate to
            Type viewModelType = response.Success ? typeof(ScoreViewModel) : typeof(LoginRegisterViewModel);

            // Navigate to the appropriate ViewModel
            MethodInfo navigateToMethod = typeof(INavigationService).GetMethod("NavigateTo");
            MethodInfo navigateMethod = navigateToMethod.MakeGenericMethod(viewModelType);
            navigateMethod.Invoke(navigationService, null);

            // Show the main window
            mainWindow.Show();

            // Call the base method
            base.OnStartup(e);
        }
    }
}