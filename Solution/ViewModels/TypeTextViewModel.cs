using Solution.Helpers;
using Solution.Services;
using Solution.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Solution.ViewModels
{
  public class CharacterEventCommand : ICommand
  {
    private readonly Action<object> _execute;
    private readonly Func<object, bool> _canExecute;

    public event EventHandler CanExecuteChanged
    {
      add { CommandManager.RequerySuggested += value; }
      remove { CommandManager.RequerySuggested -= value; }
    }

    public CharacterEventCommand(Action<object> execute, Func<object, bool> canExecute = null)
    {
      _execute = execute ?? throw new ArgumentNullException(nameof(execute));
      _canExecute = canExecute;
    }
    public bool CanExecute(object parameter)
    {
      return _canExecute == null || _canExecute(parameter);
    }

    public void Execute(object parameter)
    {
        private WebserverService _webserverService;
      _execute(parameter);
    }
  }

  public class TypeTextViewModel : BaseViewModel
  {

    public DispatcherTimer timer;
    public Stopwatch stopWatch;

    public double _amountOfCorrectChars;
    public double AmountOfCorrectChars
    {
      get { return _amountOfCorrectChars; }
      set
      {
        _amountOfCorrectChars = value;
        OnPropertyChanged(nameof(AmountOfCorrectChars));
      }
    }

    public double _amountOfTypedChars;
    public double AmountOfTypedChars
    {
      get { return _amountOfTypedChars; }
      set
      {
        _amountOfTypedChars = value;
        OnPropertyChanged(nameof(AmountOfTypedChars));
      }
    }

    public double _amountOfTypedWords;
    public double AmountOfTypedWords
    {
      get { return _amountOfTypedWords; }
      set
      {
        _amountOfTypedWords = value;
        OnPropertyChanged(nameof(AmountOfTypedWords));
      }
    }

    public double _weight;
    public int _score;
    public int Score
    {
      get { return _score; }
      set
      {
        _score = value;
        OnPropertyChanged(nameof(Score));
      }
    }

    public int _wpm;
    public int Wpm
    {
      get { return _wpm; }
      set
      {
        _wpm = value;
        OnPropertyChanged(nameof(Wpm));
        // Console.WriteLine("WordsPerMinute changed in typing model: " + value);
      }
    }
    public int _cpm;
    public int Cpm
    {
      get { return _cpm; }
      set
      {
        _cpm = value;
        OnPropertyChanged(nameof(Cpm));
      }
    }
    public int _accuracy;
    public int Accuracy
    {
      get { return _accuracy; }
      set
      {
        _accuracy = value;
        OnPropertyChanged(nameof(Accuracy));
      }
    }

    //Slaat stopwatch value op en wordt gebruikt om te binden aan een label
    private string _elapsedTime = String.Empty;
    public string ElapsedTime {
      get {
        return _elapsedTime;
      }
      set {
        _elapsedTime = value;
        OnPropertyChanged(nameof(ElapsedTime));
      }
    }

    private readonly ITextUpdater? _textUpdater;
    //public ObservableCollection<FormattedTextLine> Lines { get; set; }

        public ICommand PressChar { get; private set; }
        public NavRelayCommand NavigateToMultiplayerResultsView { get; set; }
    public ICommand MyCommand { get; private set; }



    public ICommand PressBackspace { get; private set; }
    private string _textCache;
    private readonly ApiClient apiClient;

    public TypeTextViewModel(INavigationService navigation,IDataService passTestStats, ApiClient client)
    {
      apiClient = client;
      Navigation = navigation;
      NavigateToTestResultsView = new NavRelayCommand(o => { Navigation.NavigateTo<TestResultsViewModel>(); }, o => true);
      NavigateToMultiplayerResultsView = new NavRelayCommand(o => { Navigation.NavigateTo<MultiplayerResultsViewModel>(); }, o => true);
      
      this.passTestStats = passTestStats;
      _weight = 0.6;
      MyCommand = new RelayCommand(ExecuteMyCommand);
      PressChar = new CharacterEventCommand(ProcessChar);
      PressBackspace = new RelayCommand(DeleteCharacter);
      tempList = new List<int> { };
      TheText = new List<string> { "Text", "could", "not", "be", "loaded" };
      _textCache = "";
      UserInput = new List<string> { "" };
      if (passTestStats.Text != null && !passTestStats.Equals(_textCache))
      {
        TheText = new List<string>(passTestStats.Text.Split(new char[] {' ', '\n'}, StringSplitOptions.RemoveEmptyEntries));
        UserInput.Clear();
        UserInput.Add("");
        _textCache = passTestStats.Text;
        updateInput(true);
      }

      stopWatch = new Stopwatch();
      timer = new DispatcherTimer();
      //live timer event
      timer.Tick += timer_Tick;

      ITextUpdater? _textUpdater = ServiceLocator.GetTextUpdater();
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

    public NavRelayCommand NavigateToTestResultsView { get; set; }

    private void updateText(List<Word> words, bool resetWordWrap = false)
    {
      if (passTestStats.Text != null && !passTestStats.Text.Equals(_textCache))
      {
        TheText = new List<string>(passTestStats.Text.Split(new char[] {' ', '\n'}, StringSplitOptions.RemoveEmptyEntries));
        UserInput.Clear();
        UserInput.Add("");
        _textCache = passTestStats.Text;
        updateInput(true);
      }

      ITextUpdater? _textUpdater = ServiceLocator.GetTextUpdater();
      if (_textUpdater != null)
      {
        _textUpdater.updateText(words, resetWordWrap);
      }
    }
    public List<string> TheText;
    public List<string> UserInput;
    public List<int> tempList;
    public List<int> intList = new List<int> { };
    public List<Word> myList = new List<Word> { };
    private readonly IDataService passTestStats;
    private ScoreViewModel scoreViewModel;


    private void ProcessChar(object parameter)
    {
      stopWatch.Start();
      timer.Start();
      /*var text = parameter as string;
      if (text != null)
      {
          tempList.Add(0);
          updateText(new List<Word>
          {
              new Word("temp", tempList),
          });
      } else
      {
          updateText(new List<Word>
          {
              new Word("no text", new List<int> {}),
          });
      }*/
      var text = parameter as string;
      if (text == " ")
      {
        UserInput.Add("");
      } else if (text?.Length == 1)
      {
        UserInput[UserInput.Count - 1] = UserInput[UserInput.Count - 1] + text;
      }
      updateInput();
    }


    private async void updateInput(bool resetWordWrap = false)
    {
      myList = new List<Word> { };
      for (int w = 0; w < TheText.Count; w++)
      {
        string word = TheText[w];
        string typedWord = "";
        if (w < UserInput.Count)
        {
            stopWatch.Start();
            timer.Start();
            /*var text = parameter as string;
            if (text != null)
            {
                tempList.Add(0);
                updateText(new List<Word>
                {
                    new Word("temp", tempList),
                });
            } else
            {
                updateText(new List<Word>
                {
                    new Word("no text", new List<int> {}),
                });
            }*/
            var text = parameter as string;
            if (text == " ")
            {
                UserInput.Add("");
            }
            else if (text?.Length == 1)
            {
                UserInput[UserInput.Count - 1] = UserInput[UserInput.Count - 1] + text;
            }
            updateInput();
        }
        intList = new List<int> { };

        private async void updateInput(bool resetWordWrap = false)
        {
            myList = new List<Word> { };
            for (int w = 0; w < TheText.Count; w++)
            {
                string word = TheText[w];
                string typedWord = "";
                if (w < UserInput.Count)
                {
                    typedWord = UserInput[w];
                }
                intList = new List<int> { };

                for (int i = 0; i < word.Length; i++)
                {
                    //char character = word[i];
                    if (i < typedWord.Length)
                    {
                        if (typedWord[i].Equals(word[i]))
                        {
                            intList.Add(0);
                        }
                        else
                        {
                            intList.Add(1);
                        }
                    }
                    else if (w < UserInput.Count - 1)
                    {
                        intList.Add(2);
                    }
                }

                if (typedWord.Length > word.Length)
                {
                    for (int i = word.Length; i < typedWord.Length; i++)
                    {
                        intList.Add(3);
                    }
                    word = word + typedWord.Substring(word.Length, typedWord.Length - word.Length);
                }

                myList.Add(new Word(word, intList));
            }
            updateText(myList, resetWordWrap);
            string lastWord = TheText[TheText.Count - 1].TrimEnd(new char[] { ' ', '\n', '\r' });
            System.Diagnostics.Debug.WriteLine(lastWord);
            if (UserInput.Count > TheText.Count || (UserInput.Count == TheText.Count &&
                                                     UserInput[UserInput.Count - 1].Length == TheText[TheText.Count - 1].TrimEnd(new char[] { ' ', '\n', '\r' }).Length &&
                                                     GetLastChar.GetLastCharacter(UserInput[TheText.Count - 1]) ==
                                                     GetLastChar.GetLastCharacter(TheText[TheText.Count - 1].TrimEnd(new char[] { ' ', '\n', '\r' }))))
            {
                StopTimer();
                CalculateScore();
                ResetData();
                if (passTestStats.Multiplayer == true)
                {

                    WebserverService.Instance.SendFinishMessage(50, 99);
                    
                    NavigateToMultiplayerResultsView.Execute(null);

                }
                else
                {
                    NavigateToTestResultsView.Execute(null);
                }
                // text finished
            }
        }

        if (typedWord.Length > word.Length)
        {
            if (UserInput[UserInput.Count - 1].Length > 0)
            {
                UserInput[UserInput.Count - 1] = UserInput[UserInput.Count - 1].Substring(0, UserInput[UserInput.Count - 1].Length - 1);
                updateInput();
            }
            else if (UserInput.Count > 1)
            {
                UserInput.RemoveAt(UserInput.Count - 1);
                updateInput();
            }
        }

        public void StopTimer()
        {
            stopWatch.Stop();
          for (int i = word.Length; i < typedWord.Length; i++)
          {
            intList.Add(3);
          }
          word = word + typedWord.Substring(word.Length, typedWord.Length - word.Length);
        }

        myList.Add(new Word(word, intList));
      }
      updateText(myList, resetWordWrap);
      string lastWord = TheText[TheText.Count - 1].TrimEnd(new char[] { ' ', '\n', '\r' });
      System.Diagnostics.Debug.WriteLine(lastWord);
      if ( UserInput.Count > TheText.Count || (UserInput.Count == TheText.Count &&
                                               UserInput[UserInput.Count-1].Length == TheText[TheText.Count-1].TrimEnd(new char[] { ' ', '\n', '\r' }).Length &&
                                               GetLastChar.GetLastCharacter(UserInput[TheText.Count - 1]) ==
                                               GetLastChar.GetLastCharacter(TheText[TheText.Count - 1].TrimEnd(new char[] { ' ', '\n', '\r' }))))
      {
        StopTimer();
        await CalculateScore();
        ResetData();
        NavigateToTestResultsView.Execute(null);
        // text finished
      }
    }

    private void ExecuteMyCommand()
    {
      ITextUpdater? _textUpdater = ServiceLocator.GetTextUpdater();
      updateInput();
    }

    public void DeleteCharacter()
    {
      if (UserInput[UserInput.Count-1].Length > 0) {
        UserInput[UserInput.Count-1] = UserInput[UserInput.Count-1].Substring(0, UserInput[UserInput.Count - 1].Length - 1);
        updateInput();
      } else if (UserInput.Count > 1) {
        UserInput.RemoveAt(UserInput.Count - 1);
        updateInput();
      }
    }

    public void StopTimer()
    {
      stopWatch.Stop();
    }

    public async Task CalculateScore()
    {
      // nog ff in aparta method stoppen
      string[] timeParts = ElapsedTime.Split(':');
      int minutes = int.Parse(timeParts[0]);
      int seconds = int.Parse(timeParts[1]);
      int milliseconds = int.Parse(timeParts[2]);
      double totalSeconds = minutes * 60 + seconds + milliseconds / 1000D;
      // Bereken WPM

      foreach (var VARIABLE in myList)
      {
        foreach (var chars in VARIABLE.Indices)
        {
          if (chars == 0)
          {
            _amountOfCorrectChars++;
            _amountOfTypedChars++;
          }
          if (chars == 1 || chars == 3)
          {
            _amountOfTypedChars++;
          }
        }
      }

      Wpm = Convert.ToInt32((UserInput.Count / totalSeconds) * 60); // WPM = (aantal woorden / tijd in minuten)
      Cpm = Convert.ToInt32((_amountOfTypedChars / totalSeconds) * 60);
      Accuracy = Convert.ToInt32(_amountOfCorrectChars / _amountOfTypedChars * 100); // via api score terugkrijgen en opsturen van data
      ApiResponse response = await apiClient.SaveScore(Accuracy, Cpm, Wpm);
            Score = Convert.ToInt32(_wpm * (1 - _weight) + _accuracy * _weight);
            passTestStats.Score = Score;
      //data passen
      passTestStats.Accuracy = Accuracy;
      passTestStats.Cpm = Cpm;
      passTestStats.Wpm = Wpm;
      passTestStats.AmountOfCorrectChars = _amountOfCorrectChars;
      passTestStats.AmountOfTypedChars = _amountOfTypedChars;
      passTestStats.AmountOfTypedWords = UserInput.Count;
      passTestStats.ElapsedTime = ElapsedTime;

      passTestStats.Score = response.Score.score;

    }

    public void ResetData()
    {
      myList.Clear();
      intList.Clear();
      tempList.Clear();
      UserInput = new List<string> { "" };
      AmountOfCorrectChars = default;
      AmountOfTypedChars = default;
      AmountOfTypedWords = default;
      Score = default;
      Wpm = default;
      Cpm = default;
      Accuracy = default;
      stopWatch.Reset();

      TimeSpan ts = stopWatch.Elapsed;
    }

    private void timer_Tick(object sender, EventArgs e)
    {
      //Als stopwatch runt
      if (stopWatch.IsRunning)
      {
        //Haalt time span op en format deze
        TimeSpan ts = stopWatch.Elapsed;
        ElapsedTime = String.Format("{0:00}:{1:00}:{2:00}",
          ts.Minutes, ts.Seconds, ts.Milliseconds / 1);
      }
    }
  }
}
