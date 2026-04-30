using UnityEngine;

public interface IStatsComponent
{
    Stats Stats { get; }
}

[ExecuteInEditMode]
public class StatsComponent : MonoBehaviour, IStatsComponent
{
    public Stats stats;
    Stats IStatsComponent.Stats => stats;

    private void OnValidate()
    {
        stats.HP.ForceNotify();
    }
}
