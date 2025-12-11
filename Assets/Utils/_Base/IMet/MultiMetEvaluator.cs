using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
#endif

[System.Serializable]
public class MultiMetEvaluator<T> : IDisposable where T : class, IMet
{
    T[] checks = Array.Empty<T>();
    public ObservableValue<bool> AllMet = new ObservableValue<bool>();

    bool initialized;
    bool subscribed;

    bool metValueOnEmpty = false;
    bool includeInactive = false;

    public MultiMetEvaluator(GameObject root, bool metValueOnEmpy = false, bool includeInactive = false)
    {
        Init(root, metValueOnEmpy, includeInactive);
    }

    /// <summary>
    /// Looks for all IMet components in the given GameObject and its children.
    /// Does NOT subscribe automatically.
    /// </summary>
    public void Init(GameObject root, bool metValueOnEmpty = false, bool includeInactive = false)
    {
        //Store parameters
        this.metValueOnEmpty = metValueOnEmpty;
        this.includeInactive = includeInactive;

        // If we were already subscribed, clean up first
        UnsubscribeInternal();


        if (root == null)
        {
            checks = Array.Empty<T>();
            initialized = false;
            AllMet.Value = this.metValueOnEmpty;
            return;
        }

        checks = root.GetComponentsInChildren<T>(includeInactive);
        initialized = true;
        AllMet.Value = this.metValueOnEmpty;
    }

    /// <summary>
    /// Subscribes to all checks and evaluates initial value.
    /// </summary>
    public void Subscribe()
    {
        if (!initialized) return;
        if (subscribed) return;
        subscribed = true;

        foreach (var check in checks)
        {
            if (check == null) continue;
            check.IsMet.OnValueChanged += OnCheckValueChanged;
        }

        Recalculate(forceNotify: true);
    }

    /// <summary>
    /// Unsubscribes from all checks.
    /// </summary>
    public void Unsubscribe()
    {
        if (!initialized) return;
        UnsubscribeInternal();
    }

    void UnsubscribeInternal()
    {
        if (!subscribed || checks == null)
        {
            subscribed = false;
            return;
        }

        for (int i = 0; i < checks.Length; i++)
        {
            var check = checks[i];
            if (check == null) continue;
            check.IsMet.OnValueChanged -= OnCheckValueChanged;
        }

        subscribed = false;
    }

    void OnCheckValueChanged(bool newValue)
    {
        Recalculate(true);
    }


    void Recalculate(bool forceNotify = false)
    {
        bool newValue = EvaluateAll();

        if (!initialized || newValue != AllMet.Value)
        {
            initialized = true;
            AllMet.Value = newValue;
            return;
        }
        else if (forceNotify) AllMet.Notify();
    }

    bool EvaluateAll()
    {
        if (checks == null || checks.Length == 0) return metValueOnEmpty;

        for (int i = 0; i < checks.Length; i++)
        {
            var check = checks[i];
            if (check == null) continue;
            if (!check.IsMet.Value)
                return false;
        }

        return true;
    }

    public void Dispose()
    {
        UnsubscribeInternal();
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(MultiMetEvaluator<>), true)]
public class MultiMetEvaluatorDrawer : PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        // Find AllMet._value
        var allMetProp = property.FindPropertyRelative("AllMet");
        var boolProp = allMetProp?.FindPropertyRelative("_value");

        // Build: "<Owner>_<Field>_AllMet"  (field display name is already "Requirements", etc.)
        string niceFieldName = property.displayName.Replace(" ", "");
        string label = $"{niceFieldName}_AllMet";

        return boolProp != null
            ? new PropertyField(boolProp, label)
            : new Label($"{label}: missing AllMet/_value");
    }
}
#endif