using Solution.Helpers;
using Solution.Services;
using Solution.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.VisualBasic.ApplicationServices;

namespace Solution.ViewModels
{
  /*public class FormattedTextLine
  {
      public ObservableCollection<Run> TextRuns { get; set; }

      public FormattedTextLine()
      {
          TextRuns = new ObservableCollection<Run>();
      }

      public void AddRun(string text, SolidColorBrush color)
      {
          TextRuns.Add(new Run(text) { Foreground = color });
      }
  }*/

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
      _execute(parameter);
    }
  }

  public class TypeTextViewModel : BaseViewModel
  {

    public DispatcherTimer timer;
    public Stopwatch stopWatch;
    

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

    public ICommand MyCommand { get; private set; }

    public ICommand PressChar { get; private set; }

    public ICommand PressBackspace { get; private set; }

    public TypeTextViewModel()
    {
      MyCommand = new RelayCommand(ExecuteMyCommand);
      PressChar = new CharacterEventCommand(ProcessChar);
      PressBackspace = new RelayCommand(DeleteCharacter);
      tempList = new List<int> { };
      TheText = new List<string> { "Hello", "my", "beautiful", "world!" };
      UserInput = new List<string> { "" };

      stopWatch = new Stopwatch();
      timer = new DispatcherTimer();
      //live timer event
      timer.Tick += timer_Tick;

      ITextUpdater? _textUpdater = ServiceLocator.GetTextUpdater();
    }

    private void updateText(List<Word> words)
    {
      ITextUpdater? _textUpdater = ServiceLocator.GetTextUpdater();
      if (_textUpdater != null)
      {
        _textUpdater.updateText(words);
      }
    }

    public List<string> TheText;
    public List<string> UserInput;
    public List<int> tempList;

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

    
    private void updateInput()
    {
      List<Word> myList = new List<Word> { };
      for (int w = 0; w < TheText.Count; w++)
      {
        string word = TheText[w];
        string typedWord = "";
        if (w < UserInput.Count)
        {
          typedWord = UserInput[w];
        }
        List<int> intList = new List<int> { };

        for (int i = 0; i < word.Length; i++)
        {
          //char character = word[i];
          if (i < typedWord.Length)
          {
            if (typedWord[i].Equals(word[i]))
            {
              intList.Add(0);
            } else
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
        
        if ( UserInput.Count > TheText.Count || (UserInput.Count == TheText.Count &&
                                                 UserInput[UserInput.Count-1].Length == TheText[TheText.Count-1].Length &&
                                                 GetLastChar.GetLastCharacter(UserInput[TheText.Count - 1]) ==
                                                 GetLastChar.GetLastCharacter(TheText[TheText.Count - 1])))
        {
          StopTimer();
          // text finished
        }

      }
      updateText(myList);
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
      
      string[] timeParts = ElapsedTime.Split(':');
      int minutes = int.Parse(timeParts[0]);
      int seconds = int.Parse(timeParts[1]);
      int milliseconds = int.Parse(timeParts[2]);
      double totalSeconds = minutes * 60 + seconds + milliseconds / 1000D;
      
      int amountOfWords = TheText.Count; // Controleer of dit het juiste aantal woorden is in jouw context
      // Bereken WPM

      int amountOfCharacters = 0;
      foreach (var text in TheText)
      {
        amountOfCharacters += text.Length;
      }
      
      int wpm = Convert.ToInt32((amountOfWords / totalSeconds) * 60); // WPM = (aantal woorden / tijd in minuten)
      int cpm = Convert.ToInt32((amountOfCharacters / totalSeconds) * 60);
      
      Console.WriteLine($"Words per minute: {wpm}");
      Console.WriteLine($"Chars per minute: {cpm}");
    }

    void timer_Tick(object sender, EventArgs e)
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
