
using UnityEngine;

public class DoDamage : MonoBehaviour
{
    [SerializeField] private float damage = 1f;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        IDamageable damageable = collision.gameObject.GetComponentInParent<IDamageable>();

        damageable?.TakeDamage(damage, gameObject);
     }
}
