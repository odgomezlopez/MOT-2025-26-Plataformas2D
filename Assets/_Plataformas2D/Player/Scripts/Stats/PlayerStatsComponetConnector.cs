using UnityEngine;

public class PlayerStatsComponetConnector : MonoBehaviour, IStatsComponent
{
    public Stats Stats => GameData.Instance.GetComponent<PlayerStatsComponent>().Stats;
}
