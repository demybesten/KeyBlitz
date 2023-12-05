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

        public RelayCommand NavigateToLeaderboardViewCommand { get; set; }
        public RelayCommand NavigateToKBViewCommand { get; set; }
        public RelayCommand NavigateToScoreViewCommand { get; set; }
        public RelayCommand NavigateToAccountViewCommand { get; set; }

        public MainViewModel(INavigationService navService)
        {
            Navigation = navService;
            NavigateToLeaderboardViewCommand = new RelayCommand(o => { Navigation.NavigateTo<LeaderboardViewModel>(); }, o => true);
            NavigateToKBViewCommand = new RelayCommand(o => { Navigation.NavigateTo<KBViewModel>(); }, o => true);
            NavigateToScoreViewCommand = new RelayCommand(o => { Navigation.NavigateTo<ScoreViewModel>(); }, o => true);
            NavigateToAccountViewCommand = new RelayCommand(o => { Navigation.NavigateTo<AccountViewModel>(); }, o => true);
        }
    }
}
