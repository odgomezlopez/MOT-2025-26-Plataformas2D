using System;
using UnityEngine;

public class MultiMetEvaluator<T> : IDisposable where T : class, IMet
{
    T[] checks = Array.Empty<T>();

    public bool AllMet { get; private set; }

    /// <summary>Fires when the aggregated result (AllMet) changes.</summary>
    public event Action<bool> OnAllMetChanged;

    bool initialized;
    bool subscribed;

    bool defaultValueOnEmpy = false;


    /// <summary>
    /// Looks for all ICheck components in the given GameObject and its children.
    /// Does NOT subscribe automatically.
    /// </summary>
    public void Init(GameObject root, bool def = false)
    {
        // If we were already subscribed, clean up first
        UnsubscribeInternal();

        defaultValueOnEmpy = def;

        if (root == null)
        {
            checks = Array.Empty<T>();
            initialized = false;
            AllMet = defaultValueOnEmpy;
            return;
        }

        // Grab all ICheck in this hierarchy (active and inactive)
        checks = root.GetComponentsInChildren<T>();//includeInactive: true
        initialized = false;
        AllMet = false;
    }

    /// <summary>
    /// Subscribes to all checks and evaluates initial value.
    /// </summary>
    public void Subscribe()
    {
        if (subscribed) return;

        if (checks == null) checks = Array.Empty<T>();

        foreach (var check in checks)
        {
            if (check == null) continue;
            check.OnMetChanged += OnCheckValueChanged;
        }

        subscribed = true;
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
            check.OnMetChanged -= OnCheckValueChanged;
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

        if (!initialized || forceNotify || newValue != AllMet)
        {
            initialized = true;
            AllMet = newValue;
            OnAllMetChanged?.Invoke(AllMet);
        }
    }

    bool EvaluateAll()
    {
        if (checks == null || checks.Length == 0) return defaultValueOnEmpy;

        for (int i = 0; i < checks.Length; i++)
        {
            var check = checks[i];
            if (check == null) continue;
            if (!check.IsMet)
                return false;
        }

        return true;
    }

    public void Dispose()
    {
        UnsubscribeInternal();
    }
}
