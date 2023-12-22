using Solution.Helpers;
using Solution.Services;
using Solution.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Text.Json;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Numerics;
using static Solution.ViewModels.MultiplayerViewModel;

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
    private ClientWebSocket _webSocket;
    private MultiplayerViewModel _multiplayerViewModel;
    private string _lobbystatus;

    public string LobbyStatus
    {
        get { return _lobbystatus; }
        set
        {
            if (_lobbystatus != value)
            {
                _lobbystatus = value;
                OnPropertyChanged(nameof(LobbyStatus));
            }
        }
    }
   
    private ObservableCollection<string> _playerNames;

    public ObservableCollection<string> PlayerNames
    {
        get { return _playerNames; }
        set
        {
            if (_playerNames != value)
            {
                _playerNames = value;
                OnPropertyChanged(nameof(PlayerNames));
            }
        }
    }
    public NavRelayCommand NavigateToNewTestView { get; set; }


    public MultiplayerResultsViewModel(IEnumerable<Player> players)
    {
        Uri serverUri = new Uri("ws://161.97.129.111:6969");

    }


    





  
}

