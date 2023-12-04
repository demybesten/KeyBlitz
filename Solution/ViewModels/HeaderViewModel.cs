using System.Windows.Input;
using Solution.Helpers;
using Solution.Services;

namespace Solution.ViewModels
{
    public class HeaderViewModel : BaseViewModel
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
        public RelayCommand NavigateToAccountViewCommand { get; set; }
        public RelayCommand NavigateToScoreViewCommand { get; set; }
        public RelayCommand NavigateToKBViewCommand { get; set; }

        public HeaderViewModel(INavigationService navService)
        {
            Navigation = navService;
            NavigateToLeaderboardViewCommand = new RelayCommand(o => { Navigation.NavigateTo<LeaderboardViewModel>(); }, o => true);
            NavigateToAccountViewCommand = new RelayCommand(o => { Navigation.NavigateTo<AccountViewModel>(); }, o => true);
            NavigateToScoreViewCommand = new RelayCommand(o => { Navigation.NavigateTo<ScoreViewModel>(); }, o => true);
            NavigateToKBViewCommand = new RelayCommand(o => { Navigation.NavigateTo<KBViewModel>(); }, o => true);
        }
    }
}

