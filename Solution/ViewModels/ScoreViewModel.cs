using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Effects;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Solution.Helpers;
using Solution.Services;

namespace Solution.ViewModels;

public class ScoreViewModel : BaseViewModel
{
  private readonly ApiClient apiClient;
  private readonly IDataService passTestStats;
  public SeriesCollection ChartSeries { get; set; }
  private List<Score> ScoreList { get; set; }
  private bool _isDataLoaded = false;
  private readonly Timer _updateTimer;


  public ScoreViewModel(INavigationService navigation, SendPrompt sendPrompt, IDataService passTestStats, ApiClient client)
  {
    _updateTimer = new Timer(UpdateData, null, TimeSpan.Zero, TimeSpan.FromSeconds(15)); // Update every 15 seconds VERANDEREN NAAR LANGERE TIJD?


    apiClient = client;
    this.passTestStats = passTestStats;
    Navigation = navigation;
    NavigateToNewTestView = new NavRelayCommand(o => { Navigation.NavigateTo<NewTestViewModel>(); }, o => true);
    NavigateToMultiplayerView = new NavRelayCommand(o => { Navigation.NavigateTo<MultiplayerViewModel>(); }, o => true);
    NavigateToTypeTextView = new NavRelayCommand(o => { Navigation.NavigateTo<TypeTextViewModel>(); }, o => true);

    _sendPrompt = sendPrompt;
    ShowPopupCommand = new RelayCommand(ShowPopup);
    HidePopupCommand = new RelayCommand(HidePopup);

    _textLength = 20;
    ComplexityLevels.Add("basic");
    ComplexityLevels.Add("average");
    ComplexityLevels.Add("advanced");
    TextTypes.Add("story");
    TextTypes.Add("sentences");
    TextTypes.Add("words");
    Languages.Add("english");
    Languages.Add("dutch");
    Languages.Add("german");
    Languages.Add("french");
    ChartFilters.Add("last week");
    ChartFilters.Add("last month");
    ChartFilters.Add("last year");
    ChartFilters.Add("all time");

    Task.Run(async () =>
    {
      ScoreList = await GetPlayerScores();
      Console.WriteLine(ScoreList.Count);

      Application.Current.Dispatcher.Invoke(() =>
      {
        var dates = new List<DateTime> { };

        ChartSeries = new SeriesCollection();

        LineSeries lineSeries = new LineSeries
        {
          PointGeometrySize = 15,
          Values = new ChartValues<ObservablePoint>()
        };

        for (int i = 0; i < ScoreList.Count; i++)
        {
          double yValue = Convert.ToDouble(ScoreList[i].score);
          lineSeries.Values.Add(new ObservablePoint(i, yValue));
          Console.WriteLine($"X: {i}, Y: {yValue}");
          dates.Add(ScoreList[i].date);
          Console.WriteLine(dates[i]);
        }

        ChartSeries.Add(lineSeries);
        DateLabels = dates.Select(d => d.ToString("dd-MM-yyyy")).ToList();

        // Zorgt ervoor dat de UI van de applicatie wordt aangepast
        OnPropertyChanged(nameof(ChartSeries));
        OnPropertyChanged(nameof(DateLabels));
      });
    });
    InitializeAsync(); // runs calculatescores for ui binding

    SendPromptCommand = new RelayCommand(async () => await SendPrompt(), () => true);
  }

  private async Task<List<Score>> GetPlayerScores()
  {
    var response = await apiClient.GetPlayerScores();
    return response.ScoreList;
  }



//     private readonly IDataService passTestStats;
//     private readonly ApiClient apiClient;
//     private List<Score> ScoreList;
//     public ScoreViewModel(INavigationService navigation, SendPrompt sendPrompt, IDataService passTestStats, ApiClient client)
//     {


//         apiClient = client;
//         this.passTestStats = passTestStats;
//         Navigation = navigation;

//         ScoreList = new List<Score>();

//         NavigateToNewTestView = new NavRelayCommand(o => { Navigation.NavigateTo<NewTestViewModel>(); }, o => true);
//         NavigateToMultiplayerView = new NavRelayCommand(o => { Navigation.NavigateTo<MultiplayerViewModel>(); }, o => true);
//         NavigateToTypeTextView = new NavRelayCommand(o => { Navigation.NavigateTo<TypeTextViewModel>(); }, o => true);

//         _sendPrompt = sendPrompt;
//         ShowPopupCommand = new RelayCommand(ShowPopup);
//         HidePopupCommand = new RelayCommand(HidePopup);

//         _textLength = 20;
//         ComplexityLevels.Add("basic");
//         ComplexityLevels.Add("average");
//         ComplexityLevels.Add("advanced");
//         TextTypes.Add("story");
//         TextTypes.Add("sentences");
//         TextTypes.Add("words");
//         Languages.Add("english");
//         Languages.Add("dutch");
//         Languages.Add("german");
//         Languages.Add("french");

//         SendPromptCommand = new RelayCommand(async () => await SendPrompt(), () => true);
//     }

    private SendPrompt _sendPrompt;

    public INavigationService _Navigation;

  // public RelayCommand SendPromptCommand { get; set; }

    public INavigationService Navigation
    {
        get => _Navigation;
        set
        {
            _Navigation = value;
            OnPropertyChanged();
        }
    }
    public NavRelayCommand NavigateToNewTestView { get; set; }
    public NavRelayCommand NavigateToMultiplayerView { get; set; }
    public NavRelayCommand NavigateToTypeTextView { get; set; }

    private string _apiKey;

    public RelayCommand SendPromptCommand { get; }

    private bool ContainsNumber(string value)
    {
        return value.Any(char.IsDigit);
    }

    private string _errorMessage;

    public string ErrorMessage
    {
        get { return _errorMessage; }
        set
        {
            _errorMessage = value;
            OnPropertyChanged(nameof(ErrorMessage));
        }
    }


  string closestString = null;

    private string _responseText;
    private string[] ResponseTextArray;

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

    private int _textLength;

    public int TextLength
    {
        get { return _textLength; }
        set
        {
            _textLength = value;
            OnPropertyChanged(nameof(TextLength));
        }
    }

    private bool _showLoading = false;

    public bool ShowLoading
    {
        get { return _showLoading; }
        set
        {
            _showLoading = value;
            OnPropertyChanged(nameof(ShowLoading));
        }
    }

    private ObservableCollection<string> _complexityLevels = new ObservableCollection<string>();
    public ObservableCollection<string> ComplexityLevels
    {
        get { return _complexityLevels; }
        set
        {
            _complexityLevels = value;
            OnPropertyChanged(nameof(ComplexityLevels));
        }
    }

    private ObservableCollection<string> _textTypes = new ObservableCollection<string>();

    public ObservableCollection<string> TextTypes
    {
        get { return _textTypes; }
        set
        {
            _textTypes = value;
            OnPropertyChanged(nameof(TextTypes));
        }
    }

    private string _textType = "story";

    public string TextType
    {
        get { return _textType; }
        set
        {
            _textType = value;
            OnPropertyChanged(nameof(TextType));
        }
    }

    private string _complexityLevel = "basic";

    public string ComplexityLevel
    {
        get { return _complexityLevel; }
        set
        {
            _complexityLevel = value;
            OnPropertyChanged(nameof(ComplexityLevel));
        }
    }

    private ObservableCollection<string> _languages = new ObservableCollection<string>();

    public ObservableCollection<string> Languages
    {
        get { return _languages; }
        set
        {
            _languages = value;
            OnPropertyChanged(nameof(Languages));
        }
    }

    private string _language = "english";

    public string Language
    {
        get { return _language; }
        set
        {
            if (/*!ContainsNumber(value)*/ true)
            {
                ErrorMessage = "";
                _language = value;
            }
            else
            {
                ErrorMessage = "Language input can't contain numbers!";
            }

            OnPropertyChanged(nameof(Language));
        }
    }

    private string _textSubject = "random";

    public string TextSubject
    {
        get { return _textSubject; }
        set
        {
            if (/*!ContainsNumber(value)*/ true)
            {
                ErrorMessage = "";
                _textSubject = value;
            }
            else
            {
                ErrorMessage = "Subject input can't contain numbers!";
            }

            OnPropertyChanged(nameof(TextSubject));
        }
    }

    public async Task SendPrompt()
    {
        ShowLoading = true;
        ResponseText = await _sendPrompt.GeneratePrompt(TextSubject,TextType,TextLength,ComplexityLevel,Language);
        if (ResponseText != "" && ResponseText != null)
        {
            ShowLoading = false;
            HidePopup();
            passTestStats.Text = ResponseText;
            NavigateToTypeTextView.Execute(null);
        }
    }

    private int _averageWPM;
    public int AverageWPM
    {
        get { return _averageWPM; }
        set
        {
            _averageWPM = value;
            OnPropertyChanged(nameof(AverageWPM));

        }
    }

    private int _averageCPM;
    public int AverageCPM
    {
        get { return _averageCPM; }
        set
        {
            _averageCPM = value;
            OnPropertyChanged(nameof(AverageCPM));

        }
    }

    private int _averageAccuracy;
    public int AverageAccuracy
    {
        get { return _averageAccuracy; }
        set
        {
            _averageAccuracy = value;
            OnPropertyChanged(nameof(AverageAccuracy));
        }
    }

    private int _testsTaken;
    public int TestsTaken
    {
        get { return _testsTaken; }
        set
        {
            _testsTaken = value;
            OnPropertyChanged(nameof(TestsTaken));

        }
    }

    private int _mpGamesPlayed;
    public int MpGamesPlayed
    {
        get { return _mpGamesPlayed; }
        set
        {
            _mpGamesPlayed = value;
            OnPropertyChanged(nameof(MpGamesPlayed));
        }
    }

    private int _mpGamesWon;
    public int MpGamesWon
    {
        get { return _mpGamesWon; }
        set
        {
            _mpGamesWon = value;
            OnPropertyChanged(nameof(MpGamesWon));
        }
    }


    private int mpGamesPlayed;
    private int mpGamesWon;
    private int totalCpm;
    private int totalWpm;
    private int totalAccuracy;
    public async void  InitializeAsync() // runs calculate in constructor
    {
        await calculateScores();
    }
    public async Task calculateScores()
    {
        var response = await apiClient.GetPlayerScores();
        ScoreList = response.ScoreList;



        Application.Current.Dispatcher.Invoke(() => // updates ui values
        {

            foreach (var score in ScoreList) // calculate totals
            {
                totalCpm += score.cpm;
                totalWpm += score.wpm;
                totalAccuracy += score.accuracy;
                if (score.multiplayerId != 0)
                {
                    mpGamesPlayed++;
                }
            }
            //calculate averages
            AverageAccuracy = totalAccuracy / ScoreList.Count;
            AverageCPM = totalCpm / ScoreList.Count;
            AverageWPM = totalWpm / ScoreList.Count;
            MpGamesPlayed = mpGamesPlayed;
            TestsTaken = ScoreList.Count;
        });
        //reset total values
        mpGamesPlayed = 0;
        mpGamesPlayed = 0;
        totalCpm = 0;
        totalWpm = 0;
        totalAccuracy = 0;
    }

    private async void UpdateData(object state) // timer update function
    {
        InitializeAsync();
    }

    private bool _isPopupVisible;

    public bool IsPopupVisible
    {
        get { return _isPopupVisible; }
        set
        {
            if (_isPopupVisible != value)
            {
                _isPopupVisible = value;
                OnPropertyChanged(nameof(IsPopupVisible));
                OnPropertyChanged(nameof(BlurEffect));
            }
        }
    }

  //   public Effect BlurEffect => IsPopupVisible ? new BlurEffect { Radius = 5 } : null;
  //
  public ICommand ShowPopupCommand { get; }

  public ICommand HidePopupCommand { get; }


  public Effect BlurEffect => IsPopupVisible ? new BlurEffect { Radius = 5 } : null;

  private void HidePopup()
  {
    IsPopupVisible = false;
  }
  private void ShowPopup()
  {
    IsPopupVisible = true;
  }
  //
  public ICommand LoadDataCommand { get; }


  private List<string> _dateLabels;
  public List<string> DateLabels
  {
    get => _dateLabels;
    set
    {
      _dateLabels = value;
      OnPropertyChanged(nameof(DateLabels));
    }
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
    }
  }
}

