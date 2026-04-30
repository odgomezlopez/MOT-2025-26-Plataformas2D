using UnityEngine;

[ExecuteInEditMode]
public class EnemyStatsComponent : MonoBehaviour, IStatsComponent
{
    public EnemyStats stats;
    public Stats Stats => stats;

    private void OnValidate()
    {
        stats.HP.ForceNotify();
    }
}
