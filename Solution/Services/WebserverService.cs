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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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

            // _ = Connect();
        }
        public WebserverService(IDataService passTestStats, ScoreViewModel scoreViewModel)
        {
            passTestStats.Multiplayer = true;
            _webSocket = new ClientWebSocket();
            _scoreViewModel = scoreViewModel;
            //_ = Connect();
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
        private static WebserverService _instance;


        public static WebserverService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new WebserverService();
                }
                return _instance;
            }
        }
        private ObservableCollection<Player> _players;
        private IDataService passTestStats;

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
        public WebSocketState ConnectionState => _webSocket.State;

        public async Task Connect()
        {
            try
            {
                Uri serverUri = new Uri("ws://161.97.129.111:6969");

                await _webSocket.ConnectAsync(serverUri, CancellationToken.None);
                await SendAuthenticationRequest();
                await LobbyUpdates();
                //MessageBox.Show("connectie gemaakt");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fout: {ex.Message}", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        public async Task SendAuthenticationRequest()
        {
            try
            {
                var buffer = new byte[1024];
                var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    string authRequest = Encoding.UTF8.GetString(buffer, 0, result.Count);

                    if (authRequest.Contains("\"type\":\"request\"") && authRequest.Contains("\"request\":\"auth\""))
                    {
                        await RespondToAuthentication();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error receiving authentication request: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task RespondToAuthentication()
        {
            try
            {
                string authTokenResponse = @"{
                    ""type"": ""auth"",
                    ""data"": {
                        ""token"": ""ditiseentoken""
                    }
                }";

                byte[] authTokenResponseBytes = Encoding.UTF8.GetBytes(authTokenResponse);
                await _webSocket.SendAsync(new ArraySegment<byte>(authTokenResponseBytes), WebSocketMessageType.Text, true, CancellationToken.None);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error responding to authentication request: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public async Task LobbyUpdates()
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

                        ProcessLobbyUpdate(lobbyUpdate);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error responding to lobby updates: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void ProcessLobbyUpdate(string lobbyUpdate)
        {
            try
            {

                var lobbyData = JsonConvert.DeserializeObject<LobbyUpdate>(lobbyUpdate);

                LobbyStatus = lobbyData.Status;
                if (LobbyStatus == "playing")
                {

                    //MessageBox.Show("test");
                }

                OnLobbyUpdateReceived(lobbyData);


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error processing lobby update: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async void ping()
        {
            string jsonFinishMessage = "ping";

            // Convert the JSON string to bytes
            byte[] finishMessageBytes = Encoding.UTF8.GetBytes(jsonFinishMessage);
            await _webSocket.SendAsync(new ArraySegment<byte>(finishMessageBytes), WebSocketMessageType.Text, true, CancellationToken.None);

        }



        public class LobbyUpdate
        {
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("players")]
            public Player[] Players { get; set; }

            [JsonProperty("timestamp")]
            public BigInteger Timestamp { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("text")]
            public string Text { get; set; }
        }


        public async void SendFinishMessage(int tijd, int accrucy)
        {
            try
            {
                var finishMessage = new
                {
                    type = "finish",
                    data = new
                    {
                        time = tijd,
                        accuracy = accrucy
                    }
                };

                // Convert the finish message to JSON
                string jsonFinishMessage = JsonConvert.SerializeObject(finishMessage);

                // Convert the JSON string to bytes
                byte[] finishMessageBytes = Encoding.UTF8.GetBytes(jsonFinishMessage);

                if (_webSocket != null && _webSocket.State == WebSocketState.Open)
                {
                    await _webSocket.SendAsync(new ArraySegment<byte>(finishMessageBytes), WebSocketMessageType.Text, true, CancellationToken.None);

                    //MessageBox.Show("Finish message sent successfully");
                }
                else
                {
                    //MessageBox.Show("WebSocket is not in the open state. Cannot send finish message.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending finish message: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public async Task LeaveLobby()
        {
            try
            {

                if (_webSocket.State == WebSocketState.Open)
                {
                    await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Leaving lobby", CancellationToken.None);
                    MessageBox.Show("lobby verlaten");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error leaving lobby: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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