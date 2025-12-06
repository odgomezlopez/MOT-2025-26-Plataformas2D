using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Interactor))]

public class GenericInteraction : MonoBehaviour, IInteraction
{
    [Header("Interaction Data")]

    [SerializeField] protected string actionName = "Interactuar";
    public string ActionName => actionName;


    [SerializeField] protected ObservableValue<bool> isEnable = new(true);
    public ObservableValue<bool> IsEnable => isEnable;

    public UnityEvent OnInteraction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Activate(GameObject activator)
    {
        if (!IsEnable.Value) return;

        OnInteraction?.Invoke();
    }
}
