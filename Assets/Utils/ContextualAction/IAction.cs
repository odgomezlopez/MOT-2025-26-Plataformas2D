using System;
using UnityEngine;

public interface IAction
{
    //Action Data
    public string ActionName { get; }
    public bool IsEnable { get; }

    event Action OnEnableChange;

    //Actions Methods
    public void Activate(GameObject activator);

}