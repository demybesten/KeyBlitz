using Solution.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Solution {
    
    public partial class App : Application {
        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);
            MainWindow window = new MainWindow();
            MainViewModel mainViewModel = new MainViewModel();
            window.DataContext = mainViewModel;
            window.Show();
        }
    }
}
