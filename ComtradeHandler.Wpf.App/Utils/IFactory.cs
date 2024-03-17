namespace ComtradeHandler.Wpf.App.Utils;

public interface IFactory<out T>
{
    T Create();
}
