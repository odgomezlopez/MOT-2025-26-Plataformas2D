using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rb;
    IGrounded2D grounded2D;
    Move2D move2D;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        grounded2D = GetComponentInChildren<IGrounded2D>();
        move2D = GetComponentInChildren<Move2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (animator == null) return;

        if(Mathf.Abs(rb.linearVelocityX) < 0.1) //Parado
            animator.SetFloat("VelocityX", 0);
        else if (!move2D.IsRunning)
            animator.SetFloat("VelocityX", 1); //Andando. Animación va a la velocidad normal
        else
            animator.SetFloat("VelocityX", 2); //Corriendo. Animación va al doble de la animación normal

        animator.SetFloat("VelocityY", rb.linearVelocityY);
        animator.SetBool("IsGrounded", grounded2D.IsGroundedRaw);
    }
}
