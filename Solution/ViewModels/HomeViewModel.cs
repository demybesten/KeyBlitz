using System.Windows.Input;
using Solution.Helpers;
using Solution.Services;

namespace Solution.ViewModels;

public class HomeViewModel: BaseViewModel
{
    public string Message => "dit is de HomeView";
    
    public ICommand NavigateNewTestCommand { get; }// eerst Command aanmaken

    public HomeViewModel(NavigationStore navigationStore)
    {
        NavigateNewTestCommand = new NavigateCommand<NewTestViewModel>(navigationStore,() => new NewTestViewModel(navigationStore));
        // NavigateCommand maken,                       ^^^^binnen<> is de model van de view waar je heen wil, hier ook^^^^^
    }
}
