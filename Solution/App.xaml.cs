using System;
using Solution.ViewModels;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Solution.Services;

namespace Solution
{
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;

        public App()
        {
            IServiceCollection services = new ServiceCollection();
            
            services.AddSingleton<MainWindow>(ServiceProvider => new MainWindow
            {
                DataContext = ServiceProvider.GetRequiredService<MainViewModel>()
            });
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<HomeViewModel>();
            services.AddSingleton<KBViewModel>();
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