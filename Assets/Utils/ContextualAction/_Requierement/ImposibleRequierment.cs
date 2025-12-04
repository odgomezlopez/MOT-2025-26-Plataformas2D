using System;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class ImposibleRequierment : MonoBehaviour, IRequirement
{
    [SerializeField] string failRequiermentMsg = "Es imposible de activar";
    public string FailRequiermentMsg => failRequiermentMsg;

    public bool IsMet => false;

    public event Action<bool> OnMetChanged;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OnMetChanged.Invoke(false);
    }
}
