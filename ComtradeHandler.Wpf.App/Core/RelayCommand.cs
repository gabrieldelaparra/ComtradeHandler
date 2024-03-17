using System.Windows.Input;

namespace ComtradeHandler.Wpf.App.Core;

public class RelayCommand : ICommand
{
    private readonly Predicate<object?>? _targetCanExecuteMethod;
    private readonly Action<object?>? _targetExecuteMethod;

    public RelayCommand(Action<object?>? executeMethod, Predicate<object?>? canExecuteMethod = null)
    {
        _targetExecuteMethod = executeMethod;
        _targetCanExecuteMethod = canExecuteMethod;
    }

    bool ICommand.CanExecute(object? parameter)
    {
        if (_targetCanExecuteMethod != null) {
            return _targetCanExecuteMethod(parameter);
        }

        if (_targetExecuteMethod != null) {
            return true;
        }

        return false;
    }

    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
    void ICommand.Execute(object? parameter)
    {
        _targetExecuteMethod?.Invoke(parameter);
    }
}
