using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rb;
    IGrounded2D grounded2D;
    Move2D move2D;
    StatsComponent stats;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        grounded2D = GetComponentInChildren<IGrounded2D>();
        move2D = GetComponentInChildren<Move2D>();
        stats = GetComponentInChildren<StatsComponent>();
    }

    //Me suscribo/desuscribo de los cambio de HP
    private void OnEnable()
    {
        if (stats) stats.stats.HP.OnValueChanged += UpdateHP;
    }

    private void OnDisable()
    {
        if (stats) stats.stats.HP.OnValueChanged -= UpdateHP;
    }

    private void UpdateHP(float current, float max, float oldValue=0)
    {
        //Si tenemos la salud maxima no hacemos nada
        if (current == max) return;

        //Si nos hemos curado, no hacemos nada, porque aún no tenemos animación
        if (current > oldValue) return;

        //Si morimos
        if (current == 0)
        {
            animator.SetTrigger("OnDie");
        }
        //Si recibimos daño
        else
        {
            animator.SetTrigger("OnHurt");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (animator == null) return;

        if(Mathf.Abs(rb.linearVelocityX) < 0.1) //Parado
            animator.SetFloat("VelocityX", 0);
        else //Moviéndose
        {
            if (move2D) 
            {
                if (!move2D.IsRunning)
                    animator.SetFloat("VelocityX", 1); //Andando. Animación va a la velocidad normal
                else
                    animator.SetFloat("VelocityX", 2); //Corriendo. Animación va al doble de la animación normal
            }
            else
                animator.SetFloat("VelocityX", 1); 
        }
   
        animator.SetFloat("VelocityY", rb.linearVelocityY);
        animator.SetBool("IsGrounded", grounded2D.IsGroundedRaw);
    }
}
