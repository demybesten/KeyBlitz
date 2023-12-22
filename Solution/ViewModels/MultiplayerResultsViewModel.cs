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
    public ObservableCollection<Player> Players
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

        await _webSocket.ConnectAsync(serverUri, CancellationToken.None);
    }


    private async void ProcessLobbyUpdate(string lobbyUpdate)
    {
        try
        {
            // Voeg logging toe om de ontvangen update te controleren

            // Deserialize JSON naar een object dat overeenkomt met het lobby-updateformaat
            var lobbyData = JsonConvert.DeserializeObject<LobbyUpdate>(lobbyUpdate);

            LobbyStatus = lobbyData.Status;
            StartTimer();
            if (LobbyStatus == "playing")
            {
                MessageBox.Show("test");
                await _scoreViewModel.SendPrompt();

            }
            foreach (var player in lobbyData.Players)
            {

                Players = new ObservableCollection<Player>(lobbyData.Players);

            }
            // Voer verdere logica uit op basis van lobby-updategegevens
            // LobbyData is een instantie van LobbyUpdate

        }
        catch (Exception ex)
        {
            // Handel fouten bij het verwerken van lobby-updategegevens af
            MessageBox.Show($"Error processing lobby update: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }





    // Definieer een klasse om lobby-updategegevens te vertegenwoordigen
    public class LobbyUpdate
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("players")]
        public Player[] Players { get; set; } // Gebruik een array in plaats van een List<Player>

        [JsonProperty("timestamp")]
        public BigInteger Timestamp { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }
}

