using ComtradeHandler.Wpf.App.Core;

namespace ComtradeHandler.Wpf.App.Dialogs;

public abstract class BaseOkCancelDialogViewModel<T> : ViewModelBase
{
    protected BaseOkCancelDialogViewModel()
    {
        OkCommand = new RelayCommand(ExecuteOkCommand, () => CanExecuteOkCommand);
        CancelCommand = new RelayCommand(ExecuteCancelCommand, () => CanExecuteCancelCommand);
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

    public void ExecuteCancelCommand()
    {
        OnExecuteCancelCommand?.Invoke();
    }

    public virtual void ExecuteOkCommand()
    {
        if (Result is not null) {
            OnExecuteOkCommand?.Invoke(Result);
        }
    }
}
