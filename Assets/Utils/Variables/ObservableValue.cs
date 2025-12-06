using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
#endif

[System.Serializable]
public class ObservableValue<T>
{
    [SerializeField] private T _value;

    public event Action<T> OnValueChanged;


    public ObservableValue() {}

    public ObservableValue(T v){
        _value = v;
    }

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



#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ObservableValue<>), useForChildren: true)]
public class ObservableValueDrawer : PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        var valueProp = property.FindPropertyRelative("_value");
        return valueProp != null
            ? new PropertyField(valueProp, property.displayName)
            : new Label("ObservableValue: missing _value");
    }
}
#endif