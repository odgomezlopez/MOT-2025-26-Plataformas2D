using System;
using UnityEngine;

public class ImposibleRequierment : MonoBehaviour, IRequirement
{
    [SerializeField] string failRequiermentMsg = "Es imposible de activar";
    public string FailRequiermentMsg => failRequiermentMsg;


    [SerializeField] private ObservableValue<bool> isMet = new(false);
    public ObservableValue<bool> IsMet => isMet;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        IsMet.Value = false;
        IsMet.Notify();
    }

}
