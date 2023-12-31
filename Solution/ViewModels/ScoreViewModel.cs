using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Effects;
using Solution.Helpers;
using Solution.Services;
using Solution.Views;
using Solution.ViewModels;

namespace Solution.ViewModels;

public class ScoreViewModel : BaseViewModel
{

  private readonly IDataService passTestStats;

  public ScoreViewModel(INavigationService navigation, SendPrompt sendPrompt, IDataService passTestStats)
  {
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

    SendPromptCommand = new RelayCommand(async () => await SendPrompt(), () => true);
  }
   
    private SendPrompt _sendPrompt;

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
    Console.WriteLine("Generating....");
    //ResponseText = await _sendPrompt.GeneratePrompt(TextSubject,TextType,TextLength,ComplexityLevel,Language);
        ResponseText = await _sendPrompt.GeneratePrompt("scheeps", "story", 20, "basic", "Dutch");
        if (ResponseText != "" && ResponseText != null)
    {
      ShowLoading = false;
      HidePopup();
      passTestStats.Text = ResponseText;
      NavigateToTypeTextView.Execute(null);
    }
  }

  public NavRelayCommand NavigateToNewTestView { get; set; }
  public NavRelayCommand NavigateToMultiplayerView { get; set; }

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

  public Effect BlurEffect => IsPopupVisible ? new BlurEffect { Radius = 5 } : null;

  public ICommand ShowPopupCommand { get; }
  private void ShowPopup()
  {
    IsPopupVisible = true;
  }
  public ICommand HidePopupCommand { get; }
  private void HidePopup()
  {
    IsPopupVisible = false;
  }
}
