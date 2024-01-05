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
            // Initialiseer de lijst van spelers
            _ = WebserverService.Instance.LobbyUpdates();

            _players = new ObservableCollection<Player>();
            _ = WebserverService.Instance.LobbyUpdates();

            // Abonneer op het event voor lobby-updates
            WebserverService.Instance.LobbyUpdateReceived += OnLobbyUpdateReceived;
        }

        private void OnLobbyUpdateReceived(object sender, LobbyUpdateEventArgs e)
        {
            MessageBox.Show("teryerte");
            var lobbyData = e.LobbyUpdate;
            // Update de lijst van spelers op basis van lobby-update
            UpdatePlayersList(lobbyData.Players, sender, e);
        }
        public ICommand StartLobbyUpdatesCommand { get; private set; }
        private async void StartLobbyUpdates()
        {
            MessageBox.Show("refresdh");
            _ = WebserverService.Instance.LobbyUpdates();
            WebserverService.Instance.LobbyUpdateReceived += OnLobbyUpdateReceived;


        }

        private void UpdatePlayersList(IEnumerable<Player> newPlayers, object sender, LobbyUpdateEventArgs e)
        {
            var lobbyData = e.LobbyUpdate;

            foreach (var player in lobbyData.Players)
            {
                // Check if the player is already in the list
                var existingPlayer = Players.FirstOrDefault(p => p.Name == player.Name);

                if (existingPlayer != null)
                {
                    // Update existing player's accuracy
                    existingPlayer.Accuracy = player.Accuracy;

                    // Show accuracy in a MessageBox
                    ShowAccuracyMessageBox(existingPlayer);
                }
                else
                {
                    // Add the new player to the list
                    Players.Add(new Player { Name = player.Name, Accuracy = player.Accuracy });

                    // Show accuracy in a MessageBox for the new player
                    ShowAccuracyMessageBox(Players.Last());
                }
            }
        }

        private void ShowAccuracyMessageBox(Player player)
        {
            if (player.Accuracy.HasValue)
            {
                MessageBox.Show($"Player: {player.Name}\nAccuracy: {player.Accuracy}%", "Player Accuracy", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show($"Player: {player.Name}\nAccuracy: Not available", "Player Accuracy", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

    }


    // ... andere logica en eigenschappen zoals Accuracy tonen op de UI

    // Implementeer de rest van je viewmodel logica hier
}
