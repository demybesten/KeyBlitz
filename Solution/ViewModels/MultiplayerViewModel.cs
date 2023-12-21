using Solution.Helpers;
using Solution.Services;
using Solution.Views;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using System.Text.Json;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Numerics;

namespace Solution.ViewModels;

public class MultiplayerViewModel : BaseViewModel, INotifyPropertyChanged
{
    private ClientWebSocket _webSocket;

    public DispatcherTimer timer;
    public Stopwatch stopWatch;
    
    private string _status;


    public event PropertyChangedEventHandler PropertyChanged;

    public string Status
    {
        get { return _status; }
        set
        {
            if (_status != value)
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }
    }
    //Slaat stopwatch value op en wordt gebruikt om te binden aan een label
    private string _elapsedTime = String.Empty;
    public string ElapsedTime
    {
        get
        {
            return _elapsedTime;
        }
        set
        {
            _elapsedTime = value;
            OnPropertyChanged(nameof(ElapsedTime));
        }
    }
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
    private ObservableCollection<Player> _players;
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
    public MultiplayerViewModel(INavigationService navigation, IDataService passTestStats)
    {

        _webSocket = new ClientWebSocket();
        Connect();
        Navigation = navigation;
        NavigateToMultiplayerResultsView = new NavRelayCommand(o => { Navigation.NavigateTo<MultiplayerResultsViewModel>(); }, o => true);

        // Initialiseer de lijst van spelers
      

        stopWatch = new Stopwatch();
        stopWatch.Start();
        stopWatch.Stop();  // Stop immediately to set the initial value

        // The remaining time will be 10 seconds initially
        stopWatch.Reset();
        stopWatch.Start();

        timer = new DispatcherTimer();
        timer.Tick += timer_Tick;
        Status = "Start";
       
    
        

    }
    public class Player
    {
        [JsonProperty("playerId")]
        public int PlayerId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        // Voeg andere eigenschappen van de speler toe indien nodig
    }
    public ICommand ConnectCommand { get; }

    public NavRelayCommand NavigateToMultiplayerResultsView { get; set; }

   
    // Andere logica voor het toevoegen/verwijderen van spelers kan hier worden toegevoegd
    // ...
  

    private void StartTimer()
    {
        if (Status == "waiting")
        {
            stopWatch.Start();
            timer.Start();
        }
    }


    private void timer_Tick(object sender, EventArgs e)
    {
        // Als stopwatch loopt
        if (stopWatch.IsRunning)
        {
            // Haal TimeSpan op en bereken de resterende tijd
            TimeSpan elapsedTime = stopWatch.Elapsed;
            TimeSpan remainingTime = TimeSpan.FromSeconds(10) - elapsedTime;

            // Als de resterende tijd kleiner is dan of gelijk is aan 0, stop de timer
            if (remainingTime <= TimeSpan.Zero)
            {
                stopWatch.Stop();
                timer.Stop();
                ElapsedTime = "00";
                NavigateToMultiplayerResultsView.Execute(null);

            }
            else
            {
                // Formatteer de resterende tijd om alleen seconden weer te geven
                ElapsedTime = String.Format("{0:00}", remainingTime.Seconds);
            }
        }
    }



    private async Task Connect()
    {
        try
        {
            Uri serverUri = new Uri("ws://161.97.129.111:6969");

            await _webSocket.ConnectAsync(serverUri, CancellationToken.None);
            await SendAuthenticationRequest();
            await LobbyUpdates();
            // Voeg hier het succesbericht toe

        }
        catch (Exception ex)
        {
            // Handel fouten hier af
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

    private void ProcessLobbyUpdate(string lobbyUpdate)
    {
        try
        {
            // Voeg logging toe om de ontvangen update te controleren

            // Deserialize JSON naar een object dat overeenkomt met het lobby-updateformaat
            var lobbyData = JsonConvert.DeserializeObject<LobbyUpdate>(lobbyUpdate);

            LobbyStatus = lobbyData.Status;
            if (LobbyStatus == "playing")
            {
                NavigateToMultiplayerResultsView.Execute(null);

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





    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
