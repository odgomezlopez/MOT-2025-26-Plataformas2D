
using UnityEngine;

public class DoDamage : MonoBehaviour
{
    [SerializeField] private float damage = 1f;
    [SerializeField] private bool destroyOnDamage=false;

    //[SerializeField] private bool instaKill = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        IDamageable damageable = collision.gameObject.GetComponentInParent<IDamageable>();
        damageable?.TakeDamage(damage, gameObject);
        if(destroyOnDamage) Destroy(gameObject);

        //if(!instaKill) damageable?.TakeDamage(damage, gameObject);
        //else damageable?.InstaKill();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable damageable = collision.gameObject.GetComponentInParent<IDamageable>();
        damageable?.TakeDamage(damage, gameObject);
        if (destroyOnDamage) Destroy(gameObject);

        //if (!instaKill) damageable?.TakeDamage(damage, gameObject);
        //else damageable?.InstaKill();
    }
}
