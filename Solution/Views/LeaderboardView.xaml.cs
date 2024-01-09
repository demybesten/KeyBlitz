using Solution.ViewModels;
using System.Windows.Controls;
using System.Windows.Input;

namespace Solution.Views;

public partial class LeaderboardView : UserControl
{
    public LeaderboardViewModel ViewModel { get; set; }

    public LeaderboardView()
    {
        InitializeComponent();
    }

    private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }
}