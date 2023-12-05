using System;
using System.Windows.Input;

namespace Solution.Helpers;

public class RelayCommand: ICommand
{
    private readonly Predicate<object> _canExecute;
    private readonly Action<object> _execute;
    
    private readonly Action __execute;
    private readonly Func<bool> __canExecute;
    public RelayCommand(Action<object> execute, Predicate<object> canExecute)
    {
        _canExecute = canExecute;
        _execute = execute;
    }
    
    public RelayCommand(Action execute, Func<bool> canExecute = null)
    {
        __execute = execute ?? throw new ArgumentNullException(nameof(execute));
        __canExecute = canExecute;
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