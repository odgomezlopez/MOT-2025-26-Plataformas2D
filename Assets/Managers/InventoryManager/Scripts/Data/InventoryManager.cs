using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Requirement: https://assetstore.unity.com/packages/tools/utilities/serialized-dictionary-243052
public class InventoryManager : MonoBehaviourSingleton<InventoryManager>
{
    #region Fields

    // Item -> Quantity
    [SerializeField] private SerializedDictionary<Item, int> data = new();

    // Stores a snapshot used to roll back on game over
    private SerializedDictionary<Item, int> save = new();

    #endregion

    #region Properties

    public bool HasSave => save.Count > 0;
    public int Count => data.Count;

    #endregion

    #region Events

    public event System.Action OnInventoryChange;

    #endregion

    #region Unity Callbacks

    private void OnEnable()
    {
        // OnDisable mirrors this exactly; if GameManager is destroyed between the two,
        // the listener is simply never removed — acceptable since both objects are gone.
        //if (GameManager.Instance)
        //{
        //    GameManager.Instance.OnGameOver.AddListener(ResetInventory);
        //    GameManager.Instance.OnWin.AddListener(SaveInventory);
        //}
    }

    private void OnDisable()
    {
        //if (GameManager.Instance)
        //{
        //    GameManager.Instance.OnGameOver.RemoveListener(ResetInventory);
        //    GameManager.Instance.OnWin.RemoveListener(SaveInventory);
        //}
    }

    #endregion

    #region Public Methods

    public void AddItemToInventory(Item itemData, int cant = 1)
    {
        if (itemData == null)
        {
            Debug.LogWarning("[InventoryManager] AddItem: itemData is null.");
            return;
        }

        if (cant <= 0)
        {
            Debug.LogWarning($"[InventoryManager] AddItem: quantity must be > 0, got {cant}.");
            return;
        }

        if (!CheckItemInventory(itemData))
            data[itemData] = cant;
        else
            data[itemData] += cant;

        OnInventoryChange?.Invoke();
    }

    public void RemoveItemFromInventory(Item itemData, int cant = 1)
    {
        if (itemData == null)
        {
            Debug.LogWarning("[InventoryManager] RemoveItem: itemData is null.");
            return;
        }

        if (cant <= 0)
        {
            Debug.LogWarning($"[InventoryManager] RemoveItem: quantity must be > 0, got {cant}.");
            return;
        }

        if (!CheckItemInventory(itemData))
            return;

        data[itemData] = Mathf.Max(0, data[itemData] - cant);

        if (data[itemData] == 0)
            data.Remove(itemData);

        OnInventoryChange?.Invoke();
    }

    public void Clear()
    {
        data.Clear();
        OnInventoryChange?.Invoke();
    }

    public bool CheckItemInventory(Item itemData)
    {
        return data.TryGetValue(itemData, out int qty) && qty > 0;
    }

    public List<KeyValuePair<Item, int>> GetSnapshot()
    {
        return data.ToList();
    }

    #endregion

    #region Save & Reset

    public void SaveInventory()
    {
        save = new SerializedDictionary<Item, int>(data);
    }

    public void ResetInventory()
    {
        if (!HasSave)
        {
            Debug.LogWarning("[InventoryManager] ResetInventory called but no save snapshot exists.");
            return;
        }

        data = new SerializedDictionary<Item, int>(save);
        OnInventoryChange?.Invoke();
    }

    #endregion
}