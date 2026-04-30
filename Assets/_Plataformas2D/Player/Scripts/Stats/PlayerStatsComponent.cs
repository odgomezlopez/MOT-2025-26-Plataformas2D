using UnityEngine;

[ExecuteInEditMode]
public class PlayerStatsComponent : MonoBehaviour, IStatsComponent
{
    public PlayerStats stats;

    public Stats Stats => stats;

    private void OnValidate()
    {
        stats.HP.ForceNotify();
    }
}
