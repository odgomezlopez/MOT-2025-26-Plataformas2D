using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MoveFoward : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed = 5f;
    public Vector2 direction = Vector2.right;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.linearVelocity = direction * speed;
    }
}