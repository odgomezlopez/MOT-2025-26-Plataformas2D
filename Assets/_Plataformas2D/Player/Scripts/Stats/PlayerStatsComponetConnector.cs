using UnityEngine;

public class PlayerStatsComponentConnector : MonoBehaviour, IStatsComponent
{
    public Stats Stats => GameData.Instance != null
        ? GameData.Instance.GetComponent<PlayerStatsComponent>()?.Stats
        : null;
}