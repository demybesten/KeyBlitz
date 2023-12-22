using Newtonsoft.Json;
using Solution.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.WebSockets;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using static Solution.Services.WebserverService;
using static Solution.ViewModels.MultiplayerViewModel;

namespace Solution.Services
{
    public class WebserverService
    {
        private ClientWebSocket _webSocket;
        private ScoreViewModel _scoreViewModel;


        public event EventHandler<LobbyUpdateEventArgs> LobbyUpdateReceived;

        protected virtual void OnLobbyUpdateReceived(LobbyUpdate lobbyUpdate)
        {
            LobbyUpdateReceived?.Invoke(this, new LobbyUpdateEventArgs(lobbyUpdate));
        }

        public WebserverService()
        {
            _webSocket = new ClientWebSocket();
        }
        public WebserverService(IDataService passTestStats)
        {
            passTestStats.Multiplayer = true;
            _webSocket = new ClientWebSocket();

        }
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
        private ObservableCollection<Player> _players;
        public event PropertyChangedEventHandler PropertyChanged;

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
        public async Task Connect()
        {
            try
            {
                // Voeg een breakpoint toe op de volgende regel
                Uri serverUri = new Uri("ws://161.97.129.111:6969");

                await _webSocket.ConnectAsync(serverUri, CancellationToken.None);
                await SendAuthenticationRequest();
                await LobbyUpdates();
                // Voeg hier het succesbericht toe
                MessageBox.Show("connectie gemaakt");
            }
            catch (Exception ex)
            {
                // Voeg een breakpoint toe op de volgende regel
                MessageBox.Show($"Fout: {ex.Message}", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private async Task SendAuthenticationRequest()
        {
            try
            {
                // Receive and handle authentication request from the server
                var buffer = new byte[1024];
                var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    string authRequest = Encoding.UTF8.GetString(buffer, 0, result.Count);

                    // Check if it's an authentication request
                    if (authRequest.Contains("\"type\":\"request\"") && authRequest.Contains("\"request\":\"auth\""))
                    {
                        // Respond with authentication token
                        await RespondToAuthentication();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle errors here
                MessageBox.Show($"Error receiving authentication request: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task RespondToAuthentication()
        {
            try
            {
                // Respond with authentication token
                string authTokenResponse = @"{
                    ""type"": ""auth"",
                    ""data"": {
                        ""token"": ""ditiseentoken""
                    }
                }";

                byte[] authTokenResponseBytes = Encoding.UTF8.GetBytes(authTokenResponse);
                await _webSocket.SendAsync(new ArraySegment<byte>(authTokenResponseBytes), WebSocketMessageType.Text, true, CancellationToken.None);

                // Show message box indicating successful connection
                //MessageBox.Show("Connected and authenticated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                // Handle errors here
                MessageBox.Show($"Error responding to authentication request: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async Task LobbyUpdates()
        {
            try
            {
                var buffer = new byte[1024];

                while (_webSocket.State == WebSocketState.Open)
                {
                    var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        string lobbyUpdate = Encoding.UTF8.GetString(buffer, 0, result.Count);

                        // Verwerk de lobby-update
                        ProcessLobbyUpdate(lobbyUpdate);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handel fouten hier af
                MessageBox.Show($"Error responding to lobby updates: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void ProcessLobbyUpdate(string lobbyUpdate)
        {
            try
            {
                // Voeg logging toe om de ontvangen update te controleren

                // Deserialize JSON naar een object dat overeenkomt met het lobby-updateformaat
                var lobbyData = JsonConvert.DeserializeObject<LobbyUpdate>(lobbyUpdate);

                LobbyStatus = lobbyData.Status;
                if (LobbyStatus == "playing")
                {
                    MessageBox.Show("test");
                    await _scoreViewModel.SendPrompt();
                }

                // Roep het event aan om de lobby-update door te geven aan de subscribers
                OnLobbyUpdateReceived(lobbyData);

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


        public async Task SendFinishMessage(int timeTaken, double accuracy)
        {
            try
            {
                // Construct the finish message
                var finishMessage = new
                {
                    type = "finish",
                    data = new
                    {
                        time = 123, // Vervang dit met de werkelijke tijdswaarde (int)
                        accuracy = 50.0 // Vervang dit met de werkelijke nauwkeurigheidswaarde (double)
                    }
                };

                // Convert the finish message to JSON
                string jsonFinishMessage = JsonConvert.SerializeObject(finishMessage);

                // Convert the JSON string to bytes
                byte[] finishMessageBytes = Encoding.UTF8.GetBytes(jsonFinishMessage);

                // Check if _webSocket is not null before attempting to use it
                if (_webSocket != null)
                {
                    // Send the finish message to the server
                    await _webSocket.SendAsync(new ArraySegment<byte>(finishMessageBytes), WebSocketMessageType.Text, true, CancellationToken.None);
                }
                else
                {
                    // Handle the case where _webSocket is null
                    MessageBox.Show("_webSocket is null");
                }
                MessageBox.Show("gelukt");
            }
            catch (Exception ex)
            {
                // Handle errors here
                MessageBox.Show($"Error sending finish message: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}

public class LobbyUpdateEventArgs : EventArgs
{
    public LobbyUpdate LobbyUpdate { get; }

    public LobbyUpdateEventArgs(LobbyUpdate lobbyUpdate)
    {
        LobbyUpdate = lobbyUpdate;
    }
}