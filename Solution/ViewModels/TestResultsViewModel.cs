using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using LiveCharts;
using Solution.Helpers;
using Solution.Services;
using Solution.Views;

namespace Solution.ViewModels
{
    public class TestResultsViewModel : BaseViewModel
    {
      public TestResultsViewModel(INavigationService navigation,IDataService passTestStats)
      {
          
          Navigation = navigation;
          NavigateToScoreView = new NavRelayCommand(o => { Navigation.NavigateTo<ScoreViewModel>(); }, o => true);
          NavigateToTypeTextView = new NavRelayCommand(o => { Navigation.NavigateTo<TypeTextViewModel>(); }, o => true);
          
        this.passTestStats = passTestStats;
        
          // Handle the data change in ViewModelB
          Wpm = this.passTestStats.Wpm;
          Cpm = this.passTestStats.Cpm;
          Score = this.passTestStats.Score;
          ElapsedTime = this.passTestStats.ElapsedTime;
          Accuracy = this.passTestStats.Accuracy;
          RemainingAccuracy = 100 - this.passTestStats.Accuracy;
          AmountOfCorrectChars = this.passTestStats.AmountOfCorrectChars;
          AmountOfTypedChars = this.passTestStats.AmountOfTypedChars;
          AmountOfCorrectWords = this.passTestStats.AmountOfCorrectWords;
          Console.WriteLine($"in test results{Wpm}");

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
      public NavRelayCommand NavigateToScoreView { get; set; }
      public NavRelayCommand NavigateToTypeTextView { get; set; }
      
      private readonly IDataService passTestStats;

        private int _wpm;
        public int Wpm {
            get {
                return _wpm;
            }
            set {
                _wpm = value;
                OnPropertyChanged(nameof(Wpm));
            }
        }

        private int _cpm;
        public int Cpm {
            get {
                return _cpm;
            }
            set {
                _cpm = value;
                OnPropertyChanged(nameof(Cpm));
            }
        }

        private string _elapsedTime;
        public string ElapsedTime {
            get {
                return _elapsedTime;
            }
            set {
                _elapsedTime = value;
                OnPropertyChanged(nameof(ElapsedTime));
            }
        }

        private int _score;
        public int Score {
            get {
                return _score;
            }
            set {
                _score = value;
                OnPropertyChanged(nameof(Score));
            }
        }
        
        private int _accuracy;
        public int Accuracy {
            get {
                return _accuracy;
            }
            set {
                _accuracy = value;
                OnPropertyChanged(nameof(AccuracyEnumerable));
            }
        }
        public ObservableCollection<int> AccuracyEnumerable => new ObservableCollection<int> { _accuracy };

        private int _remainingAccuracy;
        public int RemainingAccuracy
        {
            get {
                return _remainingAccuracy;
            }
            set {
                _remainingAccuracy = value;
                OnPropertyChanged(nameof(RemainingAccuracyEnumerable));
            }
        }
        public ObservableCollection<int> RemainingAccuracyEnumerable => new ObservableCollection<int> { _remainingAccuracy };

        private double _amountOfCorrectChars;
        public double AmountOfCorrectChars {
            get {
                return _amountOfCorrectChars;
            }
            set {
                _amountOfCorrectChars = value;
                OnPropertyChanged(nameof(AmountOfCorrectChars));
            }
        }

        private double _amountOfTypedChars;
        public double AmountOfTypedChars {
            get {
                return _amountOfTypedChars;
            }
            set {
                _amountOfTypedChars = value;
                OnPropertyChanged(nameof(AmountOfTypedChars));
            }
        }

        private double _amountOfCorrectWords;
        public double AmountOfCorrectWords {
            get {
                return _amountOfCorrectWords;
            }
            set {
                _amountOfCorrectWords = value;
                OnPropertyChanged(nameof(AmountOfCorrectWords));
            }
        }


    }
}
