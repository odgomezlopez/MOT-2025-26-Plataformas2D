using UnityEngine;

[ExecuteInEditMode]
public class StatsComponent : MonoBehaviour, IDamageable 
{
    public Stats stats;

    public void TakeDamage(float damage, GameObject org)
    {
        stats.HP.Value -= damage;
    }

    public void InstaKill()
    {
        stats.HP.Value = 0;
    }

    private void OnValidate()
    {
        stats.HP.Value = stats.HP.Value;
        stats.HP.MaxValue = stats.HP.MaxValue;
    }
}
