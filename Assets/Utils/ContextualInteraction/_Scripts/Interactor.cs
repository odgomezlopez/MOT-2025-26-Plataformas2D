using System;
using UnityEngine;
using UnityEngine.InputSystem;

//TODO. The canvas must be updated in Editor. Posibility to shown always on Editor
//TODO. Parameter to autotrigger even if InputActionReference is attached.
//TODO An Always active Interaction not relate to The player. Rename it PlayerInteractor. Other ContinuousInteration
//TODO Allo an Interactor to have multiples interaction. Put in Action the one with higher priority

[DefaultExecutionOrder(0)] // In Default Time
public class Interactor : MonoBehaviour
{
    #region Parameters
    [Header("Input")]
    [SerializeField, Tooltip("If not action is attached, the action will be triggered automatically on player enter")] private InputActionReference interactActionRef;
    public InputActionReference InteractActionRef { get => interactActionRef; }

    [Header("Behaviour")]
    [SerializeField] private MonoBehaviour switchableBehaviour; // must implement ISwitchable
    public IInteraction Action => switchableBehaviour as IInteraction;

    //=== CONDITIONS ===
    [SerializeField] private MultiMetEvaluator<ICheck> playerInAreaChecks; //Check if the Player is near the Interactor

    //=== GETTERS & EVENTS ===
    public ObservableValue<bool> IsPlayerInArea => playerInAreaChecks?.AllMet;
    public ObservableValue<bool> IsActionEnable => Action?.IsEnable;
    public ObservableValue<bool> AllRequirementMet => Action?.AllRequirementMet;


    #endregion

    #region Unity Life Cycle
    void Awake()
    {
        // Validate target
        if (switchableBehaviour != null && Action == null)
        {
            Debug.LogError($"{name}: switchableBehaviour does not implement ISwitchable.", this);
        }

        // Set up evaluator and find all checks in this hierarchy
        playerInAreaChecks = new MultiMetEvaluator<ICheck>(gameObject);
    }

    void OnEnable()
    {
        playerInAreaChecks?.Subscribe();

        if (interactActionRef?.action != null)
        {
            interactActionRef.action.performed += Execute;

            // If you're NOT using PlayerInput to enable maps, you may need this:
            // interactActionRef.action.Enable();
        }
        else
        {
            // auto-trigger when checks become true
            IsPlayerInArea.OnValueChanged += Execute;
            AllRequirementMet.OnValueChanged += Execute;
            IsActionEnable.OnValueChanged += Execute;

        }
    }

    void OnDisable()
    {
        if (interactActionRef?.action != null)
        {
            interactActionRef.action.performed -= Execute;
            // interactActionRef.action.Disable();
        }
        else
        {
            IsPlayerInArea.OnValueChanged -= Execute;
            AllRequirementMet.OnValueChanged -= Execute;
            IsActionEnable.OnValueChanged -= Execute;
        }

        playerInAreaChecks?.Unsubscribe();
    }

    void OnDestroy()
    {
        if (playerInAreaChecks != null)
        {
            playerInAreaChecks.Dispose();
            playerInAreaChecks = null;
        }
    }
    #endregion

    private void Execute(InputAction.CallbackContext context = default) => Execute();
    private void Execute(bool AllMet) => Execute();


    private void Execute()
    {
        if (Action == null) return;
        if (!IsActionEnable.Value) return;
        if (!IsPlayerInArea.Value) return;
        if (!AllRequirementMet.Value) return;

        Action.Activate(gameObject);
    }

    #region Editor utilities
    void OnValidate()
    {
        if (switchableBehaviour != null && switchableBehaviour is not IInteraction)
        {
            Debug.LogWarning($"{name}: Assigned component doesn't implement ISwitchable, resetting.", this);
            switchableBehaviour = null;
        }
    }
    #endregion
}