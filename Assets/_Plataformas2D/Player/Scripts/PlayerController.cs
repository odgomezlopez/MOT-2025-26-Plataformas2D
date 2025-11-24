using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Input controls")]
    [SerializeField] PlayerInput input;
    [SerializeField] InputActionReference moveActionRef;
    [SerializeField] InputActionReference jumpActionRef;

    //Referencias a componentes
    Run2D run2D;
    Jump2D jump2D;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if(input == null) input = FindAnyObjectByType<PlayerInput>();
        //Formas alternativas de inicializar las acciones
        //moveAction = input.actions["Move"]; //Del PlayerInput
        //move=InputSystem.actions["Move"];  //De las acciones globales.

        //Inicializo referencias
        run2D = GetComponent<Run2D>();
        jump2D = GetComponent<Jump2D>();
    }

    // Update is called once per frame
    void Update()
    {
        run2D.Move(moveActionRef.action.ReadValue<Vector2>());
        if (jumpActionRef.action.triggered) jump2D.Jump();
    }
}
