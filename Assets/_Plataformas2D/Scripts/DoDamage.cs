
using UnityEngine;

public class DoDamage : MonoBehaviour
{
    [SerializeField] public float damage = 1f;
    [SerializeField] private bool instaKill = false;
    [SerializeField] public bool destroyOnDamage=false;
    public GameObject attParent;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        DoDamageGeneric(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DoDamageGeneric(collision.gameObject);
    }

    private void DoDamageGeneric(GameObject g)
    {
        IDamageable damageable = g.GetComponentInParent<IDamageable>();
        if (damageable != null)
        {
            if (!instaKill) damageable?.TakeDamage(damage, gameObject);
            else damageable?.InstaKill();

            if (destroyOnDamage)
            {
                if (!attParent) attParent = gameObject;
                Destroy(attParent);
            }
        }
    }
}
