using UnityEngine;

public class InstaKill : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        IDamageable damageable = collision.gameObject.GetComponentInParent<IDamageable>();
        damageable.InstaKill();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable damageable = collision.gameObject.GetComponentInParent<IDamageable>();
        damageable.InstaKill();
    }
}