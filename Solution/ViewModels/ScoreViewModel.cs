using Solution.Helpers;
using Solution.Services;

namespace Solution.ViewModels;

public class ScoreViewModel : BaseViewModel
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
    
    public RelayCommand NavigateToNewTestView { get; set; }

    public ScoreViewModel(INavigationService navigation)
    {
        Navigation = navigation;
        NavigateToNewTestView = new RelayCommand(o => { Navigation.NavigateTo<NewTestViewModel>(); }, o => true);

    }

    private int _averageRPM = 126;
    public int AverageRPM
    {
        get { return _averageRPM; }
        set
        {
            _averageRPM = value;
            OnPropertyChanged(nameof(AverageRPM));
        }
    }
    
    private int _averageCPM = 618;
    public int AverageCPM
    {
        get { return _averageCPM; }
        set
        {
            _averageCPM = value;
            OnPropertyChanged(nameof(AverageCPM));
        }
    }
    
    private int _averageAccuracy = 95;
    public int AverageAccuracy
    {
        get { return _averageAccuracy; }
        set
        {
            _averageAccuracy = value;
            OnPropertyChanged(nameof(AverageAccuracy));
        }
    }
    
    private int _wordsTyped = 54932;
    public int WordsTyped
    {
        get { return _wordsTyped; }
        set
        {
            _wordsTyped = value;
            OnPropertyChanged(nameof(WordsTyped));
        }
    }
    
    private int _testsTaken = 419;
    public int TestsTaken
    {
        get { return _testsTaken; }
        set
        {
            _testsTaken = value;
            OnPropertyChanged(nameof(TestsTaken));
        }
    }
    
    private int _mpGamesPlayed = 53;
    public int MpGamesPlayed
    {
        get { return _mpGamesPlayed; }
        set
        {
            _mpGamesPlayed = value;
            OnPropertyChanged(nameof(MpGamesPlayed));
        }
    }
    
    private int _mpGamesWon = 15;
    public int MpGamesWon
    {
        get { return _mpGamesWon; }
        set
        {
            _mpGamesWon = value;
            OnPropertyChanged(nameof(MpGamesWon));
        }
    }
}