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
using static Solution.ViewModels.MultiplayerViewModel;

namespace Solution.ViewModels;

public class MultiplayerViewModel : BaseViewModel, INotifyPropertyChanged
{
    private WebserverService _webserverService;
    public DispatcherTimer timer;
    public Stopwatch stopWatch;
    
    private string _status;

    private ScoreViewModel _scoreViewModel;
    private ObservableCollection<Player> _updatedPlayers = new ObservableCollection<Player>();

    public event PropertyChangedEventHandler PropertyChanged;

    public ObservableCollection<Player> UpdatedPlayers
    {
        get { return _updatedPlayers; }
        set
        {
            if (_updatedPlayers != value)
            {
                _updatedPlayers = value;
                OnPropertyChanged(nameof(UpdatedPlayers));
            }
        }
    }

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

    public MultiplayerViewModel(INavigationService navigation, WebserverService _webserverservice, IDataService passTestStats)
    {
        Starten(_webserverservice);
        _webserverservice.LobbyUpdateReceived += OnLobbyUpdateReceived;

        passTestStats.Multiplayer = true;
        _webserverService = _webserverservice;
        Navigation = navigation;
        NavigateToTypeTextViewCommand = new NavRelayCommand(o => { Navigation.NavigateTo<TypeTextViewModel>(); }, o => true);

        // Initialiseer de lijst van spelers
        _scoreViewModel = new ScoreViewModel(navigation, new SendPrompt(), passTestStats);

        stopWatch = new Stopwatch();
        stopWatch.Start();
        stopWatch.Stop();  // Stop meteen om de initiële waarde in te stellen

        // De resterende tijd zal in eerste instantie 10 seconden zijn
        stopWatch.Reset();
        stopWatch.Start();

        timer = new DispatcherTimer();
        timer.Tick += timer_Tick;
        Status = "Start";

        // Verplaats de starten-functie hier naartoe
    }
    private void OnLobbyUpdateReceived(object sender, LobbyUpdateEventArgs e)
    {
        // Verwerk de lobby-updategegevens hier en werk de weergave bij
        var lobbyData = e.LobbyUpdate;

        // Update de Players-collectie in het MultiplayerViewModel

        foreach (var player in lobbyData.Players)
        {
            UpdatedPlayers.Add(new Player { Name = player.Name });
            MessageBox.Show(player.Name);
        }

        Players = UpdatedPlayers;
    }
    private WebserverService _webserverservice;
    
    public class Player
    {
        [JsonProperty("playerId")]
        public int PlayerId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        // Voeg andere eigenschappen van de speler toe indien nodig
    }
    public ICommand ConnectCommand { get; }

    public NavRelayCommand NavigateToTypeTextViewCommand { get; set; }
    public ObservableCollection<Player> Players { get; private set; }


    // Andere logica voor het toevoegen/verwijderen van spelers kan hier worden toegevoegd
    // ...


    private void StartTimer()
    {
       
            stopWatch.Start();
            timer.Start();
        
    }

private async void Starten(WebserverService server)
{
    try
    {
        // Roep de methode aan en wacht op voltooiing
        await server.Connect();
    }
    catch (Exception ex)
    {

    }
}



    public async void timer_Tick(object sender, EventArgs e)
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
                MessageBox.Show("test");   
               


            }
            else
            {
                // Formatteer de resterende tijd om alleen seconden weer te geven
                ElapsedTime = String.Format("{0:00}", remainingTime.Seconds);
            }
        }
    }



   


    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
