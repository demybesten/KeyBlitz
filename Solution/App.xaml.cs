// App.xaml.cs
using System.Windows;
using Solution.ViewModels;
using Solution.Views;

namespace YourNamespace
{
  public partial class App : Application
  {
    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);

      GptViewModel viewModel = new GptViewModel();
      GptView view = new GptView();

      view.DataContext = viewModel;

      view.Show();
    }
  }
}
