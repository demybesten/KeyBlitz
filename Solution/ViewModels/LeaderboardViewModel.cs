using Solution.Helpers;
using Solution.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Solution.ViewModels;
public class Persoon
{
    public int Naam { get; set; }
    public int Score { get; set; }
    public string Positie { get; set; }

}
public class LeaderboardViewModel : BaseViewModel
{
    private readonly ApiClient apiClient;
    private List<Score> leaderboardScores;
    public LeaderboardViewModel(INavigationService navigation,ApiClient client)
    {
        ChartFilters.Add("last week");
        ChartFilters.Add("last month");
        ChartFilters.Add("last year");
        ChartFilters.Add("all time");

        Personen = new ObservableCollection<Persoon>();
        Leaderboard = new List<Score>();
        
        
        apiClient = client;
        Navigation = navigation;
        NavigateToNewTestView = new NavRelayCommand(o => { Navigation.NavigateTo<NewTestViewModel>(); }, o => true);
        InitializeAsync();
    }

    public List<Score> Leaderboard;
    
    private async void InitializeAsync()
    {
        await SetLeaderboard();
    }

    private async Task SetLeaderboard()
    {
        var response = await apiClient.GetLeaderboard(FilterToTimePeriod());
        Leaderboard = response.ScoreList;
        Console.WriteLine(Leaderboard[0].score);
        
        if (Personen.Count == 0)
        {
            Personen.Add(new Persoon { Naam = Leaderboard[0].user_id, Score = Leaderboard[0].score, Positie = "1"});
            Personen.Add(new Persoon { Naam = Leaderboard[1].user_id, Score = Leaderboard[1].score, Positie = "2" });
            Personen.Add(new Persoon { Naam = Leaderboard[2].user_id, Score = Leaderboard[2].score, Positie = "3" });
            Personen.Add(new Persoon { Naam = Leaderboard[3].user_id, Score = Leaderboard[3].score, Positie = "4" });
            Personen.Add(new Persoon { Naam = Leaderboard[4].user_id, Score = Leaderboard[4].score, Positie = "5" });
        }
        else
        {
            Personen.Clear();
            Personen.Add(new Persoon { Naam = Leaderboard[0].user_id, Score = Leaderboard[0].score, Positie = "1"});
            Personen.Add(new Persoon { Naam = Leaderboard[1].user_id, Score = Leaderboard[1].score, Positie = "2" });
            Personen.Add(new Persoon { Naam = Leaderboard[2].user_id, Score = Leaderboard[2].score, Positie = "3" });
            Personen.Add(new Persoon { Naam = Leaderboard[3].user_id, Score = Leaderboard[3].score, Positie = "4" });
            Personen.Add(new Persoon { Naam = Leaderboard[4].user_id, Score = Leaderboard[4].score, Positie = "5" });
        }
    }
    
    private ObservableCollection<Persoon> _scores;
    public ObservableCollection<Persoon> Scores
    {
        get { return _scores; }
        set
        {
            _scores = value;
            OnPropertyChanged(nameof(Scores));
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

    private ObservableCollection<Persoon> _personen;

    public event PropertyChangedEventHandler PropertyChanged;

    public ObservableCollection<Persoon> Personen
    {
        get { return _personen; }
        set
        {
            _personen = value;
            OnPropertyChanged(nameof(Personen));
        }
    }
    public NavRelayCommand NavigateToNewTestView { get; set; }
    
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private ObservableCollection<string> _chartFilters = new ObservableCollection<string>();
    public ObservableCollection<string> ChartFilters
    {
        get { return _chartFilters; }
        set
        {
            _chartFilters = value;
            OnPropertyChanged(nameof(ChartFilters));
        }
    }

    private string _chartFilter = "all time";
    public string ChartFilter
    {
        get { return _chartFilter; }
        set
        {
            _chartFilter = value;
            OnPropertyChanged(nameof(ChartFilter));
            InitializeAsync();
        }
    }

    private LeaderboardTimeperiod FilterToTimePeriod()
    {
        if (ChartFilter == "last week")
        {
            return LeaderboardTimeperiod.Week;
        }

        if (ChartFilter == "last month")
        {
            return LeaderboardTimeperiod.Month;
        }

        if (ChartFilter == "last year")
        {
            return LeaderboardTimeperiod.Year;
        }

        if (ChartFilter == "all time")
        {
            return LeaderboardTimeperiod.AllTime;
        }

        return 0;
    }
}
