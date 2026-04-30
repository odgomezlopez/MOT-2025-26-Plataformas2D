using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Item", menuName = "ScriptableObjects/ItemDataScriptableObject/Default", order = 1)]
public class Item : ScriptableObject
{
    [Header("Default item info")]

    public string itemName;
    public Sprite sprite;

    [TextArea] public string description;

    [Header("Item usable info")]
    public bool usable = false;
    public string useButtonText="Usar";
    public bool deleteOnUse = true;

    //public Dictionary<string, int> stats = new();

    public virtual void Use()
    {
        Debug.Log($"Has usado el item: {itemName}");
    }

    public void RemoveOneFromInventory()
    {
        InventoryManager.Instance.RemoveItemFromInventory(this, 1);
    }
}