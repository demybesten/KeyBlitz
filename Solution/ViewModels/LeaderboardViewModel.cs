using Solution.Helpers;
using Solution.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
namespace Solution.ViewModels;
public class Persoon
{
    public string Naam { get; set; }
    public int Score { get; set; }
}
public class LeaderboardViewModel : BaseViewModel
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

    public LeaderboardViewModel(INavigationService navigation)
    {
        Navigation = navigation;
        NavigateToNewTestView = new NavRelayCommand(o => { Navigation.NavigateTo<NewTestViewModel>(); }, o => true);

    }
    public LeaderboardViewModel()
    {
        // Initialisatie van de personenlijst (voorbeeldgegevens)
        Personen = new ObservableCollection<Persoon>
        {
            new Persoon { Naam = "Alice", Score = 90 },
            new Persoon { Naam = "Bob", Score = 75 },
            new Persoon { Naam = "Charlie", Score = 85 },
            new Persoon { Naam = "David", Score = 95 },
            new Persoon { Naam = "Eva", Score = 80 }
        };

        // Uitvoeren van de LINQ-query
        var hoogScorendePersonen = from persoon in Personen
                                   where persoon.Score > 80
                                   orderby persoon.Score descending
                                   select persoon;

        Personen = new ObservableCollection<Persoon>(hoogScorendePersonen);
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
