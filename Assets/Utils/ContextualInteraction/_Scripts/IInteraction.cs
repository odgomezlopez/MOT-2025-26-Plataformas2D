using UnityEngine;

/// <summary>
/// Contract for an interactable behaviour.
/// An Interactor can query its state (enabled/requirements) and trigger it via Activate().
/// </summary>
public interface IInteraction
{
    /// <summary>
    /// Display name shown to the player (e.g., "Open", "Talk", "Pick up").
    /// </summary>
    public string ActionName { get; }

    /// <summary>
    /// Used to choose between multiple available interactions.
    /// Higher priority should win.
    /// </summary>
    public int Priority { get; } //If there are multiple possible iteration, the one with higher priority will be triggered

    // =========================
    // Conditions (reactive state)
    // =========================

    /// <summary>
    /// Whether this interaction is enabled (e.g., not on cooldown, not locked by game logic).
    /// Reactive so UI can update automatically.
    /// </summary>
    public ObservableValue<bool> IsEnable { get; }

    /// <summary>
    /// Whether all requirements to activate are currently met.
    /// Reactive so UI can show "locked"/"requires key"/etc.
    /// </summary>
    public ObservableValue<bool> AllRequirementMet { get; }

    // =========================
    // Action
    // =========================

    /// <summary>
    /// Executes the interaction.
    /// Implementations should assume Interactor already checked IsEnabled + AllRequirementsMet,
    /// but it's still good practice to guard internally.
    /// </summary>
    /// <param name="activator">The GameObject that triggered the interaction (usually the player).</param>
    public void Activate(GameObject activator);

}

