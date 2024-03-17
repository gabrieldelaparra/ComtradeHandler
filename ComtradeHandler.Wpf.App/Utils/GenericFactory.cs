namespace ComtradeHandler.Wpf.App.Utils;

public class GenericFactory<T> : IFactory<T>
{
    private readonly Func<T> _factory;

    public GenericFactory(Func<T> factory)
    {
        _factory = factory;
    }

    public T Create()
    {
        return _factory();
    }
}
