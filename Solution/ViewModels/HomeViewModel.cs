using Solution.Helpers;
using Solution.Services;

namespace Solution.ViewModels;

public class HomeViewModel : BaseViewModel
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

    public HomeViewModel(INavigationService navigation)
    {
        Navigation = navigation;
        NavigateToNewTestView = new RelayCommand(o => { Navigation.NavigateTo<NewTestViewModel>(); }, o => true);

    }
}