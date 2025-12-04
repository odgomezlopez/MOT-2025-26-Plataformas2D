using System;
using UnityEngine;

public interface IAction
{
    //Action Data
    public string ActionName { get; }
    public bool IsEnable { get; }


    //Actions Methods
    public void Activate(GameObject activator);

    event Action OnActionEnded;
}