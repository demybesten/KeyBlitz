﻿using Solution.Helpers;
using Solution.Services;

namespace Solution.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private INavigationService _navigation;

        public INavigationService Navigation
        {
            get => _navigation;
            set
            {
                _navigation = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand NavigateToHomeCommand { get; set; }
        public RelayCommand NavigateToKBViewCommand { get; set; }

        public MainViewModel(INavigationService navService)
        {
            Navigation = navService;
            NavigateToHomeCommand = new RelayCommand(o => { Navigation.NavigateTo<HomeViewModel>(); }, o => true);
            NavigateToKBViewCommand = new RelayCommand(o => { Navigation.NavigateTo<KBViewModel>(); }, o => true);
        }
    }
}
