using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Move2D : MonoBehaviour
{
    //Variables internas
    float inputX;
    bool isRunning = false;

    const float INPUT_DEADZONE = 0.1f;

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
        //if (Mathf.Abs(inputX) < INPUT_DEADZONE) inputX = 0;

        //Desactivo correr al cambiar de dirección
        if (Mathf.Abs(inputX) < INPUT_DEADZONE) isRunning = false; //TODO Permitir mantener carrera al cambiar de dirección
    }

    public void Run(InputAction.CallbackContext context = default)
    {
        isRunning = !isRunning;
    }

    public bool IsRunning => isRunning;

    //private void FixedUpdate()
    //{
    //    //Caculo mi velocidad
    //    float currentSpeed = inputX * statsComponent.stats.moveSpeed;

    //    if (isRunning) currentSpeed *= statsComponent.stats.runModifier;

    //    //Aplico la velocidad
    //    rb.linearVelocityX = currentSpeed;
    //}

    private void FixedUpdate()
    {
        // === 0. Accesos rápidos ===
        var stats = statsComponent.stats;

        bool grounded = grounded2D != null && grounded2D.IsGroundedRaw;
        bool hasInput = Mathf.Abs(inputX) > 0f; // ya aplicamos deadzone en Move

        // === 1. Calcular velocidad máxima (andar vs correr) ===
        float maxSpeed = stats.moveSpeed;
        if (isRunning) maxSpeed *= stats.runModifier;

        // === 2. Velocidad objetivo ===
        float currentVelX = rb.linearVelocityX;
        float targetSpeed = inputX * maxSpeed;

        //Comprobamos si el jugador está cambiado de dirección
        bool isTurning = hasInput && currentVelX * targetSpeed < 0f; //Si algún otro script la necesita, hacerla global.

        // === 3. Aceleración ===
        float accel = hasInput ? stats.acceleration : stats.deceleration;

        if (isTurning)
            accel *= stats.turnBoost;

        // Modificador de control en aire
        if (!grounded)
            accel *= stats.airMomentum; // < 1 → menos control en aire

        // === 4. Consideración de inercias y cambios ===

        //Bajamos la velocidad actual a 0 para mejorar el control
        if (isTurning) currentVelX = 0f;
        //if (!hasInput && grounded) currentVelX = 0f; ;

        float maxDelta = accel * Time.fixedDeltaTime;

        if (Mathf.Abs(targetSpeed - currentVelX) <= maxDelta * 2)
            currentVelX = targetSpeed;
        else
            currentVelX = Mathf.MoveTowards(currentVelX, targetSpeed, maxDelta);

        // === 5. Aplicar cambio ===
        rb.linearVelocityX = currentVelX;
    }

}
