using Solution.Helpers;
using Solution.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Solution.ViewModels;
public class Persoon
{
    public int Naam { get; set; }
    public int Score { get; set; }
    public int Positie { get; set; }

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

        if (Personen.Count != 0)
        {
            Personen.Clear();
        }
        for (int i = 0; i < 5; i++)
        {
            Console.WriteLine(i);
            Personen.Add(new Persoon { Naam = Leaderboard[i].user_id, Score = Leaderboard[i].score, Positie = i+1});
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