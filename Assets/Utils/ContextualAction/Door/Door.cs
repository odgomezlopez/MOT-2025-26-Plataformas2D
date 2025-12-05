using System;
using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour, IAction
{
    [SerializeField] string actionName = "Abrir";
    public string ActionName => actionName;


    bool active = false;

    public bool IsEnable => active;



    public UnityEvent OnOpen;

    public event Action OnEnableChange;

    public void Activate(GameObject activator)
    {
        active = true;
        //Hago desaparecer la puerta. Programa aquí las acciones que quieras, disparar animación, sonido, etc.
        OnOpen.Invoke();
        OnEnableChange.Invoke();

        gameObject.SetActive(false);//Opcional, desactivar, mover, lo que quieras.
    }
}
