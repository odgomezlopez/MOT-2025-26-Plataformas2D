using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObservableValue<T>
{
    [SerializeField] private T _value;

    public event Action<T> OnValueChanged;

    public T Value
    {
        get => _value;
        set => Set(value);
    }

    /// <summary>Sets the value and notifies listeners if it actually changed.</summary>
    public bool Set(T newValue)
    {
        if (EqualityComparer<T>.Default.Equals(_value, newValue))
            return false;

        _value = newValue;
        OnValueChanged?.Invoke(_value);
        return true;
    }

    /// <summary>Forces a notification without changing the value.</summary>
    public void Notify() => OnValueChanged?.Invoke(_value);
}
