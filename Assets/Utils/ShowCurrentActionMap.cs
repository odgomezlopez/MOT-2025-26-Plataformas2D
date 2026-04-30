using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShowCurrentActionMap : MonoBehaviour
{

    [SerializeField] string currentActionMap;
    PlayerInput playerInput;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = FindObjectOfType<PlayerInput>();    
    }

    // Update is called once per frame
    void Update()
    {
        currentActionMap = playerInput.currentActionMap.name;
    }
}
