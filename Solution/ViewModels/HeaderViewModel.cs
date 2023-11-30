using System.Windows.Input;
using Solution.Helpers;
using Solution.Services;

namespace Solution.ViewModels
{
    public class HeaderViewModel : BaseViewModel
    {
        public ICommand NavigateNewTestCommand { get; }// eerst Command aanmaken
        public ICommand NavigateKBViewCommand { get; }// eerst Command aanmaken


        public HeaderViewModel(NavigationStore navigationStore)
        {
            NavigateNewTestCommand = new NavigateCommand<NewTestViewModel>(navigationStore,() => new NewTestViewModel(navigationStore));
            NavigateKBViewCommand = new NavigateCommand<KBViewModel>(navigationStore,() => new KBViewModel(navigationStore));
        }
    }
}

