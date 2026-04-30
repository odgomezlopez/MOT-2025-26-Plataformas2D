using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ItemController2D : MonoBehaviour //MonoBehaviourSaveable Cambiar para guardado en memoria
{
    [Header("Item Data")]
    public Item itemData;
    public bool addedFlag = false;

    public void Start()
    {
        if(addedFlag==true) gameObject.SetActive(false);

        //Actualizamos la UI
        GetComponentInChildren<SpriteRenderer>().sprite = itemData.sprite;
    }

    public void AddToInventory()
    {
        InventoryManager.Instance.AddItemToInventory(itemData);
        addedFlag = true;
        gameObject.SetActive(false); //Si quieres que se guarden los objetos desactivados, en SaveManager, recuerda cambiar a true el booleano var a_Saveables = FindObjectsOfType<SaveableEntity>(true); 
    }

    //Editor
    private void OnRenderObject()
    {
        GetComponentInChildren<SpriteRenderer>().sprite = itemData.sprite;
    }
}
