using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    #region Parameters
    [Header("Input")]
    [SerializeField, Tooltip("If not action is attached, the action will be triggered automatically")] private InputActionReference interactActionRef;
    public InputActionReference InteractActionRef { get => interactActionRef; }

    [Header("Behaviour")]
    [SerializeField] private MonoBehaviour switchableBehaviour; // must implement ISwitchable
    public IAction Action => switchableBehaviour as IAction;

    //=== CONDITIONS ===
    private MultiMetEvaluator<ICheck> checkEvaluator; //Check if the Player is near the Interactor
    private MultiMetEvaluator<IRequirement> requirementEvaluator; //Check if the Player met the conditions to trigger the action.

    //=== GETTERS & EVENTS ===
    public bool IsActionEnable => Action.IsEnable;
    public event Action<bool> OnIsActionEnable;
    public bool AllChecksMet => checkEvaluator.AllMet;
    public event Action<bool> OnChecksMet;

    public bool AllRequirementsMet => requirementEvaluator.AllMet;
    public event Action<bool> OnRequirementsMet;

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
        checkEvaluator = new MultiMetEvaluator<ICheck>(gameObject);
        requirementEvaluator = new MultiMetEvaluator<IRequirement>(gameObject, true);
    }

    void OnEnable()
    {
        if (interactActionRef?.action != null)
            checkEvaluator.OnAllMetChanged += Subscribe;
        else
            checkEvaluator.OnAllMetChanged += Execute;

        checkEvaluator?.Subscribe();
        requirementEvaluator?.Subscribe();

        Subscribe();
    }

    void OnDisable()
    {
        if (interactActionRef?.action != null)
            interactActionRef.action.performed -= Execute;
        else
            checkEvaluator.OnAllMetChanged -= Execute;

        checkEvaluator?.Unsubscribe();
        requirementEvaluator?.Unsubscribe();

        Unsubscribe();
    }

    void OnDestroy()
    {
        if (checkEvaluator != null)
        {
            checkEvaluator.Dispose();
            checkEvaluator = null;
        }
    }
    #endregion

    private void Subscribe(bool value = true)
    {
        if (value != true) return;

        if (interactActionRef?.action != null)
            interactActionRef.action.performed += Execute;
    }

    private void Unsubscribe(bool value = false)
    {
        if (value != false) return;

        if (interactActionRef?.action != null)
            interactActionRef.action.performed -= Execute;
    }

    private void Execute(InputAction.CallbackContext context = default) => Execute();
    private void Execute(bool AllMet) => Execute();


    private void Execute()
    {
        if (Action == null) return;
        if (!Action.IsEnable) return;
        if (!checkEvaluator.AllMet) return;
        if (!requirementEvaluator.AllMet) return;

        Action.Activate(gameObject);
    }

    #region Editor utilities
    void OnValidate()
    {
        if (switchableBehaviour != null && switchableBehaviour is not IAction)
        {
            Debug.LogWarning($"{name}: Assigned component doesn't implement ISwitchable, resetting.", this);
            switchableBehaviour = null;
        }
    }
    #endregion
}
