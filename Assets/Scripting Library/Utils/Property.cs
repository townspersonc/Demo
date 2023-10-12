using System;

public sealed class Property<T>
{
    private T _value;
    public T Value
    {
        get => _value;
        set
        {
            _value = value;
            OnChange?.Invoke(_value);
        }
    }

    public event Action<T> OnChange;

    /// <summary>
    /// Sets value without calling OnChange action, useful for initial setting.
    /// </summary>
    /// <param name="value"></param>
    public void InitialSet(T value)
    {
        _value = value;
    }

    public Property() { }
    public Property(T value) => InitialSet(value);
}
