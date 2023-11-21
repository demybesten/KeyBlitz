using System.Windows;
using Solution.ViewModels;

namespace Solution.Views;

public partial class GptView : Window
{
  public GptView()
  {
    InitializeComponent();
    DataContext = new GptViewModel();
  }
}

