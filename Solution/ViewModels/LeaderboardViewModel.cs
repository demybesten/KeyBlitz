using Solution.Helpers;
using Solution.Services;

namespace Solution.ViewModels;

public class LeaderboardViewModel : BaseViewModel
{
    public INavigationService _Navigation;

    public INavigationService Navigation
    {
        get => _Navigation;
        set
        {
            _Navigation = value;
            OnPropertyChanged();
        }
    }

    public NavRelayCommand NavigateToNewTestView { get; set; }

    public LeaderboardViewModel(INavigationService navigation)
    {
        Navigation = navigation;
        NavigateToNewTestView = new NavRelayCommand(o => { Navigation.NavigateTo<NewTestViewModel>(); }, o => true);

    }
}
