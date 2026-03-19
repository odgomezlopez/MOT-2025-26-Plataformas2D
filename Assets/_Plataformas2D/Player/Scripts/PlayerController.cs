using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Input controls")]
    [SerializeField] PlayerInput input;
    [SerializeField] InputActionReference moveActionRef;
    [SerializeField] InputActionReference runActionRef;
    [SerializeField] InputActionReference jumpActionRef;

    [SerializeField] InputActionReference action1Ref;
    [SerializeField] InputActionReference action2Ref;
    [SerializeField] InputActionReference actionModifierRef;

    //Referencias a componentes
    Move2D move2D;
    Jump2D jump2D;
    PlayerActions playerActions;

    // Inicializaciones
    #region Inicializaciones y suscripciones
    void Awake()
    {
        if(input == null) input = FindAnyObjectByType<PlayerInput>();
        //Formas alternativas de inicializar las acciones
        //moveAction = input.actions["Move"]; //Del PlayerInput
        //move=InputSystem.actions["Move"];  //De las acciones globales.

        //Inicializo referencias
        move2D = GetComponent<Move2D>();
        jump2D = GetComponent<Jump2D>();
        playerActions = GetComponent<PlayerActions>();
    }

    private void OnEnable()
    {
        jumpActionRef.action.performed += jump2D.Jump;
        runActionRef.action.performed += move2D.Run;

        action1Ref.action.performed += playerActions.Action1;
        action2Ref.action.performed += playerActions.Action2;

        actionModifierRef.action.performed += playerActions.ActivarModifiador;
        actionModifierRef.action.canceled += playerActions.DesactivarModifiador;
    }

    private void OnDisable()
    {
        jumpActionRef.action.performed -= jump2D.Jump;
        runActionRef.action.performed -= move2D.Run;

        action1Ref.action.performed -= playerActions.Action1;
        action2Ref.action.performed -= playerActions.Action2;

        actionModifierRef.action.performed -= playerActions.ActivarModifiador;
        actionModifierRef.action.canceled -= playerActions.DesactivarModifiador;
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        move2D.Move(moveActionRef.action.ReadValue<Vector2>());
        //if (jumpActionRef.action.triggered) jump2D.Jump();
    }
}
