using System;
using System.Windows.Input;

namespace Solution.Helpers;

public class NavRelayCommand: ICommand
{
    private readonly Predicate<object> _canExecute;
    private readonly Action<object> _execute;
//test
    public NavRelayCommand(Action<object> execute, Predicate<object> canExecute)
    {
        _canExecute = canExecute;
        _execute = execute;
    }

    public event EventHandler CanExecuteChanged
    {
        add => CommandManager. RequerySuggested += value;
        remove => CommandManager. RequerySuggested -= value;
    }
    public bool CanExecute(object parameter)
    {
        return _canExecute(parameter);
    }

    public void Execute(object parameter)
    {
        _execute(parameter);
    }
}
