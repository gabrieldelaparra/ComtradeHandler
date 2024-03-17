using ComtradeHandler.Wpf.App.Core;

namespace ComtradeHandler.Wpf.App.Dialogs;

public abstract class BaseOkCancelDialogViewModel<T> : ViewModelBase
{
    protected BaseOkCancelDialogViewModel()
    {
        OkCommand = new RelayCommand(ExecuteOkCommand, (object? parameter) => CanExecuteOkCommand);
        CancelCommand = new RelayCommand(ExecuteCancelCommand, (object? parameter) => CanExecuteCancelCommand);
    }

    protected string CancelButtonText { get; set; } = "Cancel";
    public RelayCommand CancelCommand { get; set; }
    protected virtual bool CanExecuteCancelCommand => true;
    protected virtual bool CanExecuteOkCommand => true;
    public string DialogTitle { get; set; } = "Base Ok/Cancel Dialog";
    protected string OkButtonText { get; set; } = "Ok";
    public RelayCommand OkCommand { get; set; }
    public Action? OnExecuteCancelCommand { get; set; }
    public Action<T>? OnExecuteOkCommand { get; set; }
    public T? Result { get; set; }

    private void ExecuteCancelCommand(object? parameter)
    {
        OnExecuteCancelCommand?.Invoke();
    }

    protected virtual void ExecuteOkCommand(object? parameter)
    {
        if (Result is not null) {
            OnExecuteOkCommand?.Invoke(Result);
        }
    }
}
