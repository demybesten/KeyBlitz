using Solution.Helpers;
using Solution.Services;
using Solution.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Windows.Input;
using System.Net.WebSockets;

namespace Solution.ViewModels
{
    public class MultiplayerResultsViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private ObservableCollection<Player> _players;

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

        public MultiplayerResultsViewModel()
        {
                StartLobbyUpdatesCommand = new RelayCommand(StartLobbyUpdates);
                _ = WebserverService.Instance.LobbyUpdates();

                _players = new ObservableCollection<Player>();
                _ = WebserverService.Instance.LobbyUpdates();

                WebserverService.Instance.LobbyUpdateReceived += OnLobbyUpdateReceived;

         //   GenerateDummyData();
        }

        private void OnLobbyUpdateReceived(object sender, LobbyUpdateEventArgs e)
        {
            // MessageBox.Show("teryerte");
            var lobbyData = e.LobbyUpdate;
            UpdatePlayersList(lobbyData.Players, sender, e);
        }

        public ICommand StartLobbyUpdatesCommand { get; private set; }
        private async void StartLobbyUpdates()
        {
            // MessageBox.Show("refresdh");
            _ = WebserverService.Instance.LobbyUpdates();
            WebserverService.Instance.LobbyUpdateReceived += OnLobbyUpdateReceived;
        }

        private void UpdatePlayersList(IEnumerable<Player> newPlayers, object sender, LobbyUpdateEventArgs e)
        {
            var lobbyData = e.LobbyUpdate;

            foreach (var player in lobbyData.Players)
            {
                var existingPlayer = Players.FirstOrDefault(p => p.Name == player.Name);

                if (existingPlayer != null)
                {
                    existingPlayer.Accuracy = player.Accuracy;

                    ShowAccuracyMessageBox(existingPlayer);
                }
                else
                {
                    Players.Add(new Player { Name = player.Name, Accuracy = player.Accuracy });

                    ShowAccuracyMessageBox(Players.Last());
                }
            }

            Players = new ObservableCollection<Player>(Players.OrderByDescending(p => p.Accuracy));
        }
        private RelayCommand _leaveLobbyCommand;

        public RelayCommand LeaveLobbyCommand
        {
            get
            {
                return _leaveLobbyCommand ?? (_leaveLobbyCommand = new RelayCommand(
                    async () => await WebserverService.Instance.LeaveLobby()
                )); ;
            }
        }
        private void GenerateDummyData()
        {
            // Generate dummy data for testing
            var dummyPlayers = new List<Player>
    {
        new Player { Name = "DummyPlayer1", Accuracy = 80 },
        new Player { Name = "DummyPlayer2", Accuracy = 70 },
        new Player { Name = "DummyPlayer3", Accuracy = 90 },
        new Player { Name = "DummyPlayer4", Accuracy = 99 },
        // Add more dummy players as needed
    };

            // Sorteer de dummygegevens op nauwkeurigheid (hoogste eerst)
            dummyPlayers = dummyPlayers.OrderByDescending(p => p.Accuracy).ToList();

            // Wijs de gesorteerde dummygegevens toe aan de Players-collectie
            Players = new ObservableCollection<Player>(dummyPlayers);
        }
        private void ShowAccuracyMessageBox(Player player)
        {
            if (player.Accuracy.HasValue)
            {
                // MessageBox.Show($"Player: {player.Name}\nAccuracy: {player.Accuracy}%", "Player Accuracy", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                // MessageBox.Show($"Player: {player.Name}\nAccuracy: Not available", "Player Accuracy", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


    }
}