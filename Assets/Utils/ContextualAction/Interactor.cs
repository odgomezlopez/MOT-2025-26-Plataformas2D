using System;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    [Header("Behaviour")]
    [SerializeField] private string interactionName = "Activar";
    public string InteractionName => interactionName;

    [SerializeField] private MonoBehaviour switchableBehaviour; // must implement ISwitchable
    private ISwitchable target => switchableBehaviour as ISwitchable;

    //Checks
    private MultiCheckEvaluator checkEvaluator;

    public bool Busy => target.IsActive;
    public event Action<bool> OnBusy;

    public bool AllMet => checkEvaluator.AllMet;
    public event Action<bool> OnAllMet;

    void Awake()
    {
        // Validate target
        if (switchableBehaviour != null && target == null)
        {
            Debug.LogError($"{name}: switchableBehaviour does not implement ISwitchable.", this);
        }

        // Set up evaluator and find all checks in this hierarchy
        checkEvaluator = new MultiCheckEvaluator();
        checkEvaluator.InitChecks(gameObject);
    }

    void OnEnable()
    {
        checkEvaluator?.Subscribe();

        checkEvaluator.OnAllMetChanged += OnAllChecksStateChanged;
        target.OnActionEnded += InteractionEnded;
    }

    void OnDisable()
    {
        checkEvaluator?.Unsubscribe();

        checkEvaluator.OnAllMetChanged -= OnAllChecksStateChanged;
        target.OnActionEnded -= InteractionEnded;
    }

    void OnDestroy()
    {
        if (checkEvaluator != null)
        {
            checkEvaluator.Dispose();
            checkEvaluator = null;
        }
    }

    void OnAllChecksStateChanged(bool allMet)
    {
        if (allMet) ExecuteInteraction();

        OnAllMet?.Invoke(allMet);
    }

    void ExecuteInteraction()
    {

        if (target == null) return;
        if (target.IsActive) return;

        target.Activate(gameObject);

        OnBusy?.Invoke(false);

    }

    void InteractionEnded()
    {
        OnBusy?.Invoke(true);
    }

    #region Editor utilities
    void OnValidate()
    {
        if (switchableBehaviour != null && switchableBehaviour is not ISwitchable)
        {
            Debug.LogWarning($"{name}: Assigned component doesn't implement ISwitchable, resetting.", this);
            switchableBehaviour = null;
        }
    }
    #endregion
}
