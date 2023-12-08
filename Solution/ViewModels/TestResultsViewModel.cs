using System;
using Solution.Services;

namespace Solution.ViewModels
{
    public class TestResultsViewModel : BaseViewModel
    {
      public TestResultsViewModel(IDataService passTestStats)
      {
        this.passTestStats = passTestStats;

        this.passTestStats.DataChanged += (sender, args) =>
        {
          // Handle the data change in ViewModelB
          _wordsPerMinute = this.passTestStats.Wpm;
          // Do something with the data
        };

      }
      private readonly IDataService passTestStats;

        private int _wordsPerMinute;
        public int WordsPerMinute {
            get {
                return _wordsPerMinute;
            }
            set {
                _wordsPerMinute = value;
                OnPropertyChanged(nameof(WordsPerMinute));
            }
        }

        private int _charactersPerMinute = 619;
        public int CharactersPerMinute {
            get {
                return _charactersPerMinute;
            }
            set {
                _charactersPerMinute = value;
                OnPropertyChanged(nameof(CharactersPerMinute));
            }
        }

        private string _timeSpent = "00:26";
        public string TimeSpent {
            get {
                return _timeSpent;
            }
            set {
                _timeSpent = value;
                OnPropertyChanged(nameof(TimeSpent));
            }
        }

        private int _totalScore = 523;
        public int TotalScore {
            get {
                return _totalScore;
            }
            set {
                _totalScore = value;
                OnPropertyChanged(nameof(TotalScore));
            }
        }

        private int _charactersTyped = 70;
        public int CharactersTyped {
            get {
                return _charactersTyped;
            }
            set {
                _charactersTyped = value;
                OnPropertyChanged(nameof(CharactersTyped));
            }
        }

        private int _charactersCorrect = 65;
        public int CharactersCorrect {
            get {
                return _charactersCorrect;
            }
            set {
                _charactersCorrect = value;
                OnPropertyChanged(nameof(CharactersCorrect));
            }
        }

        private int _wordsTyped = 20;
        public int WordsTyped {
            get {
                return _wordsTyped;
            }
            set {
                _wordsTyped = value;
                OnPropertyChanged(nameof(WordsTyped));
            }
        }


    }
}
