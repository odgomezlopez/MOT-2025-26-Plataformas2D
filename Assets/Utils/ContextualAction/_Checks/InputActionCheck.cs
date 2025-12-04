using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputActionCheck : MonoBehaviour, ICheck
{
    [Header("Input Action")]
    [Tooltip("Reference to the 'Interact' Input Action (type: Button).")]
    [SerializeField] private InputActionReference interactActionRef;
    public InputActionReference InteractActionRef { get => interactActionRef;}

    // ICheck implementation
    public bool IsMet => isPressed;


    public event Action<bool> OnCheckValueChanged;

    bool isPressed;

    void OnEnable()
    {
        if (interactActionRef == null || interactActionRef.action == null)
        {
            Debug.LogWarning($"{name}: ActionCheck has no InputActionReference assigned.", this);
            return;
        }

        var action = interactActionRef.action;

        // We consider the check true while the button is pressed/held
        action.performed += OnActionPerformed;
        action.canceled += OnActionCanceled;

        action.Enable();
    }

    void OnDisable()
    {
        if (interactActionRef == null || interactActionRef.action == null)
            return;

        var action = interactActionRef.action;

        action.performed -= OnActionPerformed;
        action.canceled -= OnActionCanceled;

        action.Disable();

        // Reset state when disabled
        if (isPressed)
            SetPressed(false);
    }

    void OnActionPerformed(InputAction.CallbackContext ctx)
    {
        SetPressed(true);
    }

    void OnActionCanceled(InputAction.CallbackContext ctx)
    {
        SetPressed(false);
    }

    void SetPressed(bool value)
    {
        if (isPressed == value) return;

        isPressed = value;
        OnCheckValueChanged?.Invoke(isPressed);
    }
}
