using System;
using System.Windows.Input;
using Solution.Helpers;
using Solution.Services;

namespace Solution.ViewModels;

public class AccountViewModel : BaseViewModel
{
    public INavigationService _Navigation;
    public ICommand ButtonCommand { get; private set; }

    public INavigationService Navigation
    {
        get => _Navigation;
        set
        {
            _Navigation = value;
            OnPropertyChanged();
        }
    }

    // public NavRelayCommand NavigateToNewTestView { get; set; }

    public AccountViewModel(INavigationService navigation)
    {
        Navigation = navigation;
        // NavigateToNewTestView = new NavRelayCommand(o => { Navigation.NavigateTo<NewTestViewModel>(); }, o => true);
        ButtonCommand = new RelayCommand(MessageBoxTest);
    }

    public async void MessageBoxTest()
    {
        ApiClient api = new ApiClient();
        // ApiResponse response = await api.Register("admin622", "password");
        await api.Login("admin", "password");
        // ApiResponse response = await api.GetUserInfo();
        // ApiResponse response = await api.GetText("story", "pirate english", "69", "dancing cupcakes", "medium");
        //await api.SaveScore(100, 42, 3000); //highscore test
        // ApiResponse response = await api.GetPlayerScores();
        ApiResponse response = await api.GetLeaderboard(LeaderboardTimeperiod.Year);
    }
}