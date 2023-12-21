using Solution.Helpers;
using Solution.Services;
using Solution.Views;
using System.Collections.ObjectModel;

namespace Solution.ViewModels;
   public class MultiplayerResultsViewModel : BaseViewModel
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
    private ObservableCollection<PlayerViewModel> _players;

    public ObservableCollection<PlayerViewModel> Players
    {
        get { return _players; }
        set
        {
            if (_players != value)
            {
                _players = value;
                OnPropertyChanged(nameof(Players));
            }
        }
    }
    public NavRelayCommand NavigateToNewTestView { get; set; }


        public MultiplayerResultsViewModel(INavigationService navigation)
        {
            Navigation = navigation;
            NavigateToNewTestView = new NavRelayCommand(o => { Navigation.NavigateTo<NewTestViewModel>(); }, o => true);
        Players = new ObservableCollection<PlayerViewModel>
    {
        new PlayerViewModel { PlayerName = "Leveloper" },
        new PlayerViewModel { PlayerName = "Xx_keymaster69_xX" },
        new PlayerViewModel { PlayerName = "LaravelVoyager" },
        new PlayerViewModel { PlayerName = "TypingPro1999" }
        // Voeg zo veel spelers toe als nodig is
    };

    }

    }

