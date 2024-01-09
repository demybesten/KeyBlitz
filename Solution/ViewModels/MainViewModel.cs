using System;
using System.Diagnostics;
using System.Windows.Input;
using Solution.Helpers;
using Solution.Services;

namespace Solution.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private INavigationService _navigation;

        public INavigationService Navigation
        {
            get => _navigation;
            set
            {
                _navigation = value;
                OnPropertyChanged();
            }
        }

        public ICommand LogOutCommand { get; }

        public void LogOut()
        {
          var path = Environment.CurrentDirectory + "\\auth.token";
          Process.Start("cmd.exe", string.Format("/c del \"{0}", path));
          Console.WriteLine($"deleted :{path}");
          NavigateToLoginViewCommand.Execute(null);
        }
        public NavRelayCommand NavigateToLeaderboardViewCommand { get; set; }
        public NavRelayCommand NavigateToTypeTextViewCommand { get; set; }
        public NavRelayCommand NavigateToScoreViewCommand { get; set; }
        public NavRelayCommand NavigateToAccountViewCommand { get; set; }
        public NavRelayCommand NavigateToTestResultsViewCommand { get; set; }

        public NavRelayCommand NavigateToMultiplayerResultsViewModelCommand { get; set; }


        public NavRelayCommand NavigateToLoginViewCommand { get; set; }


        public MainViewModel(INavigationService navService)
        {

          LogOutCommand = new RelayCommand(LogOut);


            Navigation = navService;
            NavigateToLeaderboardViewCommand = new NavRelayCommand(o => { Navigation.NavigateTo<LeaderboardViewModel>(); }, o => true);
            NavigateToTypeTextViewCommand = new NavRelayCommand(o => { Navigation.NavigateTo<TypeTextViewModel>(); }, o => true);
            NavigateToScoreViewCommand = new NavRelayCommand(o => { Navigation.NavigateTo<ScoreViewModel>(); }, o => true);
            NavigateToAccountViewCommand = new NavRelayCommand(o => { Navigation.NavigateTo<AccountViewModel>(); }, o => true);
            NavigateToTestResultsViewCommand = new NavRelayCommand(o => { Navigation.NavigateTo<TestResultsViewModel>(); }, o => true);

            NavigateToMultiplayerResultsViewModelCommand = new NavRelayCommand(o => { Navigation.NavigateTo<MultiplayerResultsViewModel>(); }, o => true);


            NavigateToLoginViewCommand = new NavRelayCommand(o => { Navigation.NavigateTo<LoginRegisterViewModel>(); }, o => true);

        }
    }
}
