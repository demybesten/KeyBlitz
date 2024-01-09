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
        private readonly IDataService _dataService;
      public TestResultsViewModel(INavigationService navigation,IDataService passTestStats)
      {
          Navigation = navigation;
          NavigateToScoreView = new NavRelayCommand(o => { Navigation.NavigateTo<ScoreViewModel>(); }, o => true);
          NavigateToTypeTextView = new NavRelayCommand(o => { Navigation.NavigateTo<TypeTextViewModel>(); }, o => true);

          _dataService = passTestStats;
      }
      public int Wpm => _dataService.Wpm;
      public int Cpm => _dataService.Cpm;
      public int Score => _dataService.Score;
      public string ElapsedTime => _dataService.ElapsedTime;
      public int Accuracy => _dataService.Accuracy;
      public double AmountOfCorrectChars => _dataService.AmountOfCorrectChars;
      public double AmountOfTypedChars => _dataService.AmountOfTypedChars;
      public double AmountOfTypedWords => _dataService.AmountOfTypedWords;
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





    }
}
