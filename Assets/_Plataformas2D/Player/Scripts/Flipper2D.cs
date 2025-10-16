using UnityEngine;

public class Flipper2D : MonoBehaviour
{
    [SerializeField] bool facingRightByDefault = true;

    SpriteRenderer sprite;
    Rigidbody2D rb;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Flip2D();
    }

    private void Flip2D()
    {
        //Gestion el Flip

        //Si nos movemos a la derecha
        if (rb.linearVelocityX > 0.1f)
        {
            sprite.flipX = !facingRightByDefault;
            //Equivalente a
            //if(facingRightByDefault) sprite.flipX = false;
            //else sprite.flipX = true;
        }

        if (rb.linearVelocityX < -0.1f)
        {
            sprite.flipX = facingRightByDefault;
            //Equivalente a
            //if(facingRightByDefault) sprite.flipX = true;
            //else sprite.flipX = false;
        }
    }

    public bool isFacingRight()
    {
        if (facingRightByDefault)
        {
            return !sprite.flipX;
        }
        else
        {
            return sprite.flipX;
        }
    }
}
