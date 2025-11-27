using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Input controls")]
    [SerializeField] PlayerInput input;
    [SerializeField] InputActionReference moveActionRef;
    [SerializeField] InputActionReference runActionRef;
    [SerializeField] InputActionReference jumpActionRef;

    //Referencias a componentes
    Move2D move2D;
    Jump2D jump2D;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if(input == null) input = FindAnyObjectByType<PlayerInput>();
        //Formas alternativas de inicializar las acciones
        //moveAction = input.actions["Move"]; //Del PlayerInput
        //move=InputSystem.actions["Move"];  //De las acciones globales.

        //Inicializo referencias
        move2D = GetComponent<Move2D>();
        jump2D = GetComponent<Jump2D>();
    }

    private void OnEnable()
    {
        jumpActionRef.action.performed += jump2D.Jump;
        runActionRef.action.performed += move2D.Run;
    }

    private void OnDisable()
    {
        jumpActionRef.action.performed -= jump2D.Jump;
        runActionRef.action.performed -= move2D.Run;
    }


    // Update is called once per frame
    void Update()
    {
        move2D.Move(moveActionRef.action.ReadValue<Vector2>());
        //if (jumpActionRef.action.triggered) jump2D.Jump();
    }
}
