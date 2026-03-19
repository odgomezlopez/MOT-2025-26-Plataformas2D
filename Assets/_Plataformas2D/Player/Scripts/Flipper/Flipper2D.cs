using System;
using UnityEngine;

public class Flipper2D : MonoBehaviour
{
    //ObservableValue<bool> mFlipped;

    [SerializeField] bool facingRightByDefault = true;
    public event Action<bool> OnFlipChanged;//Evento que se dispara cuando el valor cambia

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
        bool newFlipValue = sprite.flipX;
        if (rb.linearVelocityX > 0.1f) newFlipValue = !facingRightByDefault; //Si nos movemos a la derecha
        if (rb.linearVelocityX < -0.1f) newFlipValue = facingRightByDefault; // Si nos movemos a la izquierda

        if (newFlipValue != sprite.flipX)
        {
            sprite.flipX = newFlipValue;
            OnFlipChanged?.Invoke(isFacingRight());
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
