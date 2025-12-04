using System;
using UnityEngine;

public interface ISwitchable
{
    public bool IsActive { get; }
    public void Activate(GameObject activator);

    event Action OnActionEnded;
}