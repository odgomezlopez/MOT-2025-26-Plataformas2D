using System;
using UnityEngine;

public class MultiMetEvaluator<T> : IDisposable where T : class, IMet
{
    T[] checks = Array.Empty<T>();

    public ObservableValue<bool> AllMet = new();

    bool initialized;
    bool subscribed;

    bool defaultValueOnEmpty = false;

    public MultiMetEvaluator(GameObject root, bool metValueOnEmpy = false)
    {
        Init(root, metValueOnEmpy);
    }

    /// <summary>
    /// Looks for all IMet components in the given GameObject and its children.
    /// Does NOT subscribe automatically.
    /// </summary>
    public void Init(GameObject root, bool metValueOnEmpty = false)
    {
        // If we were already subscribed, clean up first
        UnsubscribeInternal();

        this.defaultValueOnEmpty = metValueOnEmpty;

        if (root == null)
        {
            checks = Array.Empty<T>();
            initialized = false;
            AllMet.Value = defaultValueOnEmpty;
            return;
        }

        // Grab all ICheck in this hierarchy (active and inactive)
        checks = root.GetComponentsInChildren<T>();//includeInactive: true
        initialized = true;
        AllMet.Value = defaultValueOnEmpty;
    }

    /// <summary>
    /// Subscribes to all checks and evaluates initial value.
    /// </summary>
    public void Subscribe()
    {
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
        if (checks == null || checks.Length == 0) return defaultValueOnEmpty;

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
