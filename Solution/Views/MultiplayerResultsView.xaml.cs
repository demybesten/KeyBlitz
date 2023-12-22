using Solution.ViewModels;
using System.Windows.Controls;

namespace Solution.Views;

public partial class MultiplayerResultsView : UserControl
{
    public MultiplayerResultsView()
    {
        InitializeComponent();
        DataContext = new MultiplayerResultsViewModel(null);

    }
}