using System.Windows.Input;

namespace ComtradeHandler.Wpf.App.Core;

public class RelayCommand : ICommand
{
    private readonly Predicate<object?>? _canExecute;
    private readonly Action<object?> _execute;

    public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    bool ICommand.CanExecute(object? parameter)
    {
        return _canExecute == null || _canExecute.Invoke(parameter);
    }

    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    void ICommand.Execute(object? parameter)
    {
        _execute.Invoke(parameter);
    }
}
