using System;
using Solution.Services;
using Solution.ViewModels;

namespace Solution.Helpers;

public class NavigateCommand<TViewModel>: CommandBase
where TViewModel : BaseViewModel
{
    private readonly NavigationStore _navigationStore;
    private readonly Func<TViewModel> _createViewModel;

    public NavigateCommand(NavigationStore navigationStore, Func<TViewModel> createViewModel)
    {
        _navigationStore = navigationStore;
        _createViewModel = createViewModel;
    }

    public override void Execute(object parameter)
    {
        _navigationStore.CurrentViewModel = _createViewModel();
    }
}