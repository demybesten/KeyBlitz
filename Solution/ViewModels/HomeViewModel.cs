using System.Windows.Input;
using Solution.Helpers;
using Solution.Services;

namespace Solution.ViewModels;

public class HomeViewModel: BaseViewModel
{
    public string Message => "dit is de HomeView";
    
    public ICommand NavigateNewTestCommand { get; }

    public HomeViewModel(NavigationStore navigationStore)
    {
        NavigateNewTestCommand = new NavigateCommand<NewTestViewModel>(navigationStore,() => new NewTestViewModel(navigationStore));
    }
}
