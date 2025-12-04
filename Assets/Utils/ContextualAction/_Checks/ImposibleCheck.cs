using System;
using UnityEngine;

//For Debug how the interface is shown if an action is still unaviable.
public class ImposibleCheck : MonoBehaviour, ICheck
{
    public bool IsMet => false;

    public event Action<bool> OnCheckValueChanged;

    private void Start()
    {
        OnCheckValueChanged.Invoke(false);
    }

}
