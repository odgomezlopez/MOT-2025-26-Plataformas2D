using System;
using UnityEngine;

public sealed class MultiCheckEvaluator : IDisposable
{
    ICheck[] checks = Array.Empty<ICheck>();

    public bool AllMet { get; private set; }

    /// <summary>Fires when the aggregated result (AllMet) changes.</summary>
    public event Action<bool> OnAllMetChanged;

    bool initialized;
    bool subscribed;

    /// <summary>
    /// Looks for all ICheck components in the given GameObject and its children.
    /// Does NOT subscribe automatically.
    /// </summary>
    public void InitChecks(GameObject root)
    {
        // If we were already subscribed, clean up first
        UnsubscribeInternal();

        if (root == null)
        {
            checks = Array.Empty<ICheck>();
            initialized = false;
            AllMet = false;
            return;
        }

        // Grab all ICheck in this hierarchy (active and inactive)
        checks = root.GetComponentsInChildren<ICheck>();//includeInactive: true
        initialized = false;
        AllMet = false;
    }

    /// <summary>
    /// Subscribes to all checks and evaluates initial value.
    /// </summary>
    public void Subscribe()
    {
        if (subscribed) return;

        if (checks == null) checks = Array.Empty<ICheck>();

        foreach (var check in checks)
        {
            if (check == null) continue;
            check.OnCheckValueChanged += OnCheckValueChanged;
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
            check.OnCheckValueChanged -= OnCheckValueChanged;
        }

        subscribed = false;
    }

    void OnCheckValueChanged(bool newValue)
    {
        Recalculate();
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
        if (checks == null || checks.Length == 0) return false;

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
