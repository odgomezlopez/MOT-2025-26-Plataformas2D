using UnityEngine;
using UnityEngine.InputSystem;

public class Move2D : MonoBehaviour
{
    //Variables internas
    float inputX;
    bool isRunning = false;

    //Referencia a componentes
    StatsComponent statsComponent;
    Rigidbody2D rb;
    IGrounded2D grounded2D;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isRunning = false;
        rb = GetComponent<Rigidbody2D>();
        statsComponent = GetComponent<StatsComponent>();
        grounded2D = GetComponentInChildren<IGrounded2D>();
    }

    // Update is called once per frame
    public void Move(Vector2 input)
    {
        //Capturo el input
        inputX = input.x;

        if (Mathf.Abs(inputX) < 0.1f) isRunning = false; //TODO Permitir mantener carrera al cambiar de dirección
    }

    public void Run(InputAction.CallbackContext context = default)
    {
        isRunning = true;
    }

    public bool IsRunning => isRunning;

    private void FixedUpdate()
    {
        //Caculo mi velocidad
        float currentSpeed = inputX * statsComponent.stats.moveSpeed;

        if (!grounded2D.IsGroundedRaw) currentSpeed *= statsComponent.stats.airMomentum; //TODO FIX
        else if(isRunning) currentSpeed *= statsComponent.stats.runModifier;

        //Aplico la velocidad
        rb.linearVelocityX = currentSpeed;
    }


}
