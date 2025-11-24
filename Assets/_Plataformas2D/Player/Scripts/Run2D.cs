using UnityEngine;

public class Run2D : MonoBehaviour
{
    //Variables inpesctor
    [SerializeField] private float runSpeed = 5f;

    //Variables internas
    float inputX;

    //Referencia a componentes
    Rigidbody2D rb;
    Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    public void Move(Vector2 input)
    {
        //Capturo el input
        inputX = input.x;

        //Gestiono las animaciones
        if (Mathf.Abs(rb.linearVelocityX) < 0.1) animator.Play("IDLE");
        else animator.Play("RUN");
    }

    private void FixedUpdate()
    {
        rb.linearVelocityX = inputX * runSpeed;
    }


}
