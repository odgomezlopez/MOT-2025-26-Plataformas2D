using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "new Item", menuName = "ScriptableObjects/ItemDataScriptableObject/HealingPotion", order = 1)]
public class Potion : Item
{
    [Header("Potion info")]
    [SerializeField] private int HP = 1;
    public override void Use()
    {
        GameData.Instance.GetComponent<IStatsComponent>().Stats.HP.Value += HP;
    }
}
