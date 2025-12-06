using System;
using UnityEngine;

public interface IInteraction
{
    //Action Data
    public string ActionName { get; }
    public ObservableValue<bool> IsEnable { get; }

    //Actions Methods
    public void Activate(GameObject activator);

}