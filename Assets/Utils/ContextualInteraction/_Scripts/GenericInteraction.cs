using UnityEngine;
using UnityEngine.Events;

public class GenericInteraction : MonoBehaviour, IInteraction
{
    [Header("Interaction Data")]

    [SerializeField] protected string actionName = "Interactuar";
    public string ActionName => actionName;

    [SerializeField] protected int priority = 1;
    public int Priority => priority;

    //===Enable===
    [SerializeField] protected ObservableValue<bool> isEnable = new(true);
    public ObservableValue<bool> IsEnable => isEnable;


    //===Requierments===
    [SerializeField] private MultiMetEvaluator<IRequirement> requirements;
    public ObservableValue<bool> AllRequirementMet => requirements?.AllMet;

    
    //===Events===
    [Space(10)]
    public UnityEvent OnInteraction;

    #region Unity Life Cycle
    protected virtual void Awake()
    {
        requirements = new MultiMetEvaluator<IRequirement>(gameObject, true);
    }

    protected virtual void OnEnable()
    {
        requirements?.Subscribe();
    }

    protected virtual void OnDisable()
    {
        requirements?.Unsubscribe();
    }
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Activate(GameObject activator)
    {
        if (!IsEnable.Value) return;
        if (AllRequirementMet != null && !AllRequirementMet.Value) return; // defensa extra

        OnInteraction?.Invoke();
    }
}