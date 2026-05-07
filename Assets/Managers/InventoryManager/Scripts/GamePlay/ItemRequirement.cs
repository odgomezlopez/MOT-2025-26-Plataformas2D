using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ItemRequirement : MonoBehaviour, IRequirement
{
    [Header("Required Item")]
    public Item requiredItem;
    public bool unlocked=false;
    private ObservableValue<bool> isMet;
    public ObservableValue<bool> IsMet => isMet;
    public string FailRequiermentMsg => $"Necesitas el item {requiredItem.itemName} para hacer esta acción";


    public void OnEnable()
    {
        isMet = new ObservableValue<bool>(false);
        if (InventoryManager.Instance) InventoryManager.Instance.OnInventoryChange += CheckRequirement;
        CheckRequirement();
    }

    public void OnDisable()
    {
        if(InventoryManager.Instance) InventoryManager.Instance.OnInventoryChange -= CheckRequirement;
    }

    private void CheckRequirement()
    {
        isMet.Value = InventoryManager.Instance.CheckItemInventory(requiredItem) || unlocked;
    }
}
