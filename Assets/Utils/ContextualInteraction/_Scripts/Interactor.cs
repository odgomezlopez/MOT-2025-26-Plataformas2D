using System;
using UnityEngine;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
using UnityEditor;
#endif

//TODO. The canvas must be updated in Editor. Posibility to shown always on Editor
//TODO. Parameter to autotrigger even if InputActionReference is attached.

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
    private MultiMetEvaluator<ICheck> checkEvaluator; //Check if the Player is near the Interactor
    private MultiMetEvaluator<IRequirement> requirementEvaluator; //Check if the Player met the conditions to trigger the action.

    //=== GETTERS & EVENTS ===
    public ObservableValue<bool> IsActionEnable => Action?.IsEnable;
    public ObservableValue<bool> AllChecksMet => checkEvaluator?.AllMet;
    public ObservableValue<bool> AllRequiermentsMet => requirementEvaluator?.AllMet;


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
        checkEvaluator?.Subscribe();
        requirementEvaluator?.Subscribe();

        if (interactActionRef?.action != null)
        {
            interactActionRef.action.performed += Execute;

            // If you're NOT using PlayerInput to enable maps, you may need this:
            // interactActionRef.action.Enable();
        }
        else
        {
            // auto-trigger when checks become true
            AllChecksMet.OnValueChanged += Execute;
            AllRequiermentsMet.OnValueChanged += Execute;
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
            AllChecksMet.OnValueChanged -= Execute;
            AllRequiermentsMet.OnValueChanged -= Execute;
            IsActionEnable.OnValueChanged -= Execute;
        }

        checkEvaluator?.Unsubscribe();
        requirementEvaluator?.Unsubscribe();
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

    private void Execute(InputAction.CallbackContext context = default) => Execute();
    private void Execute(bool AllMet) => Execute();


    private void Execute()
    {
        if (Action == null) return;
        if (!IsActionEnable.Value) return;
        if (!checkEvaluator.AllMet.Value) return;
        if (!requirementEvaluator.AllMet.Value) return;

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



#if UNITY_EDITOR

[CustomEditor(typeof(Interactor))]
[CanEditMultipleObjects]
public class InteractorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var i = (Interactor)target;

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Debug (runtime)", EditorStyles.boldLabel);

        using (new EditorGUI.DisabledScope(true))
        {
            bool actionEnabled = i.IsActionEnable?.Value ?? false;
            bool checksMet = i.AllChecksMet?.Value ?? false;
            bool reqsMet = i.AllRequiermentsMet?.Value ?? false;

            EditorGUILayout.Toggle("Is Action Enabled", actionEnabled);
            EditorGUILayout.Toggle("All Checks Met", checksMet);
            EditorGUILayout.Toggle("All Requirements Met", reqsMet);
        }
    }
}
#endif