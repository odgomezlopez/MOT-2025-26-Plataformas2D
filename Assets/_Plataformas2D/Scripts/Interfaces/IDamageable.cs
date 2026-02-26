using UnityEngine;

public interface IDamageable
{
    public void TakeDamage(float damage, GameObject org);
    public void InstaKill();

}
