using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

//Requierement
//https://assetstore.unity.com/packages/tools/utilities/serialized-dictionary-243052
public class InventoryManager : MonoBehaviourSingleton<InventoryManager> //MonoBehaviourSaveableSingleton<InventoryManager> Cambiar para guardado en memoria
{ 
    //Item, Cantidad
    [SerializeField] private SerializedDictionary<Item, int> data = new();
    private SerializedDictionary<Item, int> save = new();

    public int Count => data.Count;

    public event System.Action OnInventoryChange;

    public void AddItemToInventory(Item itemData, int cant = 1)
    {
        if (!CheckItemInventory(itemData))
            data[itemData] = cant;
        else
            data[itemData] += cant;

        OnInventoryChange?.Invoke();
    }

    public void RemoveItemFromInventory(Item itemData, int cant = 1)
    {
        if (CheckItemInventory(itemData))
        {
            data[itemData] -= cant;

            if (data[itemData] <= 0) data.Remove(itemData);

            OnInventoryChange?.Invoke();
        }
    }

    public void Clear()
    {
        data.Clear();
        OnInventoryChange?.Invoke();
    }

    public bool CheckItemInventory(Item itemData)
    {
        return data.ContainsKey(itemData);
    }

    public List<KeyValuePair<Item, int>> ToList()
    {
        return data.ToList();
    }

    public void SaveInventory()
    {
        save = new SerializedDictionary<Item, int>(data);
    }

    public void ResetInventory()
    {
        data = new SerializedDictionary<Item, int>(save);
        OnInventoryChange?.Invoke();
    }
}