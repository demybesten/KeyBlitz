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

        public NavRelayCommand NavigateToLeaderboardViewCommand { get; set; }
        public NavRelayCommand NavigateToTypeTextViewCommand { get; set; }
        public NavRelayCommand NavigateToScoreViewCommand { get; set; }
        public NavRelayCommand NavigateToAccountViewCommand { get; set; }
        public NavRelayCommand NavigateToTestResultsViewCommand { get; set; }

        public MainViewModel(INavigationService navService)
        {
            Navigation = navService;
            NavigateToLeaderboardViewCommand = new NavRelayCommand(o => { Navigation.NavigateTo<LeaderboardViewModel>(); }, o => true);
            NavigateToTypeTextViewCommand = new NavRelayCommand(o => { Navigation.NavigateTo<TypeTextViewModel>(); }, o => true);
            NavigateToScoreViewCommand = new NavRelayCommand(o => { Navigation.NavigateTo<ScoreViewModel>(); }, o => true);
            NavigateToAccountViewCommand = new NavRelayCommand(o => { Navigation.NavigateTo<AccountViewModel>(); }, o => true);
            NavigateToTestResultsViewCommand = new NavRelayCommand(o => { Navigation.NavigateTo<TestResultsViewModel>(); }, o => true);
        }
    }
}
