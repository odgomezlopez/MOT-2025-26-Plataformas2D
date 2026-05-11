using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

/// <summary>
/// Maps <see cref="DialogueDesign"/> values to their corresponding
/// <see cref="DialogueDesignManager"/> instances, with a fallback for unmatched ids.
/// </summary>
[Serializable]
public class DialogueDesignRegistry
{
    [Tooltip("All available dialogue designs. The first valid entry is used as fallback when a speaker's design is unmatched.")]
    [SerializedDictionary("Design", "Manager")]
    [SerializeField] private SerializedDictionary<DialogueDesign, DialogueDesignManager> entries = new();

    private DialogueDesignManager fallback;
    private bool built;

    public IEnumerable<DialogueDesignManager> All
    {
        get
        {
            EnsureBuilt();
            return entries.Values;
        }
    }

    public bool IsEmpty
    {
        get
        {
            EnsureBuilt();
            return entries.Count == 0;
        }
    }

    /// <summary>
    /// Validates entries and caches the fallback. Safe to call multiple times.
    /// </summary>
    public void Build(UnityEngine.Object context = null)
    {
        fallback = null;

        foreach (var pair in entries)
        {
            if (pair.Value == null)
            {
                Debug.LogWarning($"[{nameof(DialogueDesignRegistry)}] Design '{pair.Key}' has no manager assigned.", context);
                continue;
            }

            fallback ??= pair.Value;
        }

        built = true;
    }

    /// <summary>
    /// Returns the design matching <paramref name="id"/>, or the fallback if none matches.
    /// Returns null only if the registry is empty.
    /// </summary>
    public DialogueDesignManager Resolve(DialogueDesign? id)
    {
        EnsureBuilt();

        if (id.HasValue && entries.TryGetValue(id.Value, out var match) && match != null)
            return match;

        return fallback;
    }

    private void EnsureBuilt()
    {
        if (!built) Build();
    }
}