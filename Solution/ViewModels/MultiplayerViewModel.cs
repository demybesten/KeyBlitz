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
using System.Linq;

namespace Solution.ViewModels;

public class MultiplayerViewModel : BaseViewModel, INotifyPropertyChanged
{
    private WebserverService _webserverService;
    public DispatcherTimer timer;
    public Stopwatch stopWatch;
    private readonly IDataService passTestStats;

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
    private readonly ApiClient apiClient;

    public MultiplayerViewModel(INavigationService navigation, WebserverService _webserverservice, IDataService passTestStats, SendPrompt sendPrompt, ApiClient client)
    {
        apiClient = client;

        lobbyState = "unknown";

        this.passTestStats = passTestStats;
        NavigateToTypeTextView = new NavRelayCommand(o => { Navigation.NavigateTo<TypeTextViewModel>(); }, o => true);
        _sendPrompt = sendPrompt;
        _ = WebserverService.Instance.Connect();
        WebserverService.Instance.LobbyUpdateReceived += OnLobbyUpdateReceived;
        //_webserverservice.Connect();
        //_webserverservice.LobbyUpdateReceived += OnLobbyUpdateReceived;

        NavigateToMultiplayerResultsView = new NavRelayCommand(o => { Navigation.NavigateTo<MultiplayerResultsViewModel>(); }, o => true);

        passTestStats.Multiplayer = true;
        _webserverService = _webserverservice;
        Navigation = navigation;
        NavigateToTypeTextViewCommand = new NavRelayCommand(o => { Navigation.NavigateTo<TypeTextViewModel>(); }, o => true);

        _scoreViewModel = new ScoreViewModel(navigation, new SendPrompt(), passTestStats, client);


        stopWatch = new Stopwatch();
        timer = new DispatcherTimer();

        timer.Tick += timer_Tick;


    }
    private bool isTimerStarted = false;

    private SendPrompt _sendPrompt;
    private string _responseText;
    private string[] ResponseTextArray;

    private string lobbyState;
    public NavRelayCommand NavigateToTypeTextView { get; set; }

    public string ResponseText
    {
        get { return _responseText; }
        set
        {
            if (_responseText != value)
            {
                _responseText = value;
                OnPropertyChanged(nameof(ResponseText));
            }
        }
    }
    public async Task SendPrompt()
    {
        NavigateToTypeTextView.Execute(null);
    }
    private void OnLobbyUpdateReceived(object sender, LobbyUpdateEventArgs e)
    {
        var lobbyData = e.LobbyUpdate;
        if (lobbyData.Type == "lobby")
        {
            foreach (var player in lobbyData.Players)
            {
                // Controleer of de speler al in de collectie aanwezig is
                if (!UpdatedPlayers.Any(p => p.Name == player.Name))
                {
                    UpdatedPlayers.Add(new Player { Name = player.Name });
                }
            }
            lobbyState = lobbyData.Status;
            
        }

        Players = UpdatedPlayers;
        if (UpdatedPlayers.Count >= 2 && !isTimerStarted)
        {
            StartTimer();
            isTimerStarted = true;
        }
    }

    private WebserverService _webserverservice;


    public ICommand ConnectCommand { get; }

    public NavRelayCommand NavigateToTypeTextViewCommand { get; set; }
    public ObservableCollection<Player> Players { get; private set; }
    public NavRelayCommand NavigateToMultiplayerResultsView { get; }

    private void StartTimer()
    {
        stopWatch.Start();
        timer.Start();
    }



    public async void timer_Tick(object sender, EventArgs e)
    {
        if (stopWatch.IsRunning)
        {
            TimeSpan elapsedTime = stopWatch.Elapsed;
            TimeSpan remainingTime = TimeSpan.FromSeconds(10) - elapsedTime;

            if (remainingTime <= TimeSpan.Zero)
            {
                ElapsedTime = "00";
            }
            else
            {
                ElapsedTime = String.Format("{0:00}", remainingTime.Seconds);
            }

            if (lobbyState == "playing")
            {
                stopWatch.Stop();
                timer.Stop();
                _ = SendPrompt();
            }
            
        }
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}