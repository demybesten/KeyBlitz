using Solution.Helpers;
using Solution.Services;

namespace Solution.ViewModels;

public class AccountViewModel : BaseViewModel
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

    public NavRelayCommand NavigateToNewTestView { get; set; }

    public AccountViewModel(INavigationService navigation)
    {
        Navigation = navigation;
        NavigateToNewTestView = new NavRelayCommand(o => { Navigation.NavigateTo<NewTestViewModel>(); }, o => true);

    }
}
