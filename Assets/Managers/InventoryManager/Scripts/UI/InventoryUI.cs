using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryUI : ScreenController
{

    //Celdas
    [Header("Inventory actions")]
    [SerializeField] InputActionReference useAction;


    [Header("Inventory info")]

    List<InventoryCellController> cells;
    private InventoryCellController selectedCell;

    [Header("Elemento UI Inventario")]
    private TextMeshProUGUI nameUI;
    private TextMeshProUGUI descriptionUI;
    private Button useButtonUI;

    //Acceso directo
    InventoryManager inventory => InventoryManager.Instance;


    protected void Awake()
    {
        //Asocio elementos de info
        var info = gameObject.transform.GetChild(0).Find("Info");

        nameUI = info.Find("Name").gameObject.GetComponent<TextMeshProUGUI>();
        descriptionUI = info.Find("Description").gameObject.GetComponent<TextMeshProUGUI>();
        useButtonUI = info.Find("UseButton").gameObject.GetComponent<Button>();

        //Asocio elementos de celdas
        cells = new List<InventoryCellController>(GetComponentsInChildren<InventoryCellController>());

        //Añado la funcion al boton de añadir
        useButtonUI.onClick.AddListener(OnUseButton);

        //Activamos la navegación de UI
        EventSystem.current.firstSelectedGameObject = null;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        if(useAction) useAction.action.performed += OnUseButtonConnector;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        if (useAction) useAction.action.performed -= OnUseButtonConnector;
    }


    //Activar/Desactivar Inventario
    public override void ShowScreen()
    {
        base.ShowScreen();

        //Controles especificos del inventario

        //GameControllerBase.Instance.uiController.hudController.HideScreen();


        //Actualizo la UI del inventario

        if (cells.Count == 0) cells = new List<InventoryCellController>(GetComponentsInChildren<InventoryCellController>());
        UpdateCellsItems();

        //Selecciono el primer elemento
        if (inventory.Count != 0)
        {
            SelectItem(cells[0]);
            EventSystem.current.SetSelectedGameObject(cells[0].gameObject);
        }
    }

    //Funcionalidades especificas
    public override void HideScreen()
    {
        base.HideScreen();
        //Controles especificos del inventario
        //GameControllerBase.Instance.uiController.hudController.ShowScreen();
        EventSystem.current.SetSelectedGameObject(null);

    }

    public void OnUseButtonConnector(InputAction.CallbackContext context = default)
    {
        OnUseButton();
    }

    public void OnUseButton()
    {
        //Control
        if (!selectedCell.item.usable) return;

        //Uso el item
        var item = selectedCell.item;
        item.Use();

        //Reduzco la cantidad del inventario
        if (item.deleteOnUse) inventory.RemoveItemFromInventory(item);

        //Recargo la UI
        UpdateCellsItems();

        //Detecto el elemento seleccionado actual
        if (inventory.Count != 0)
        {
            if (selectedCell.item?.itemName == item.itemName) return;
            else EventSystem.current.SetSelectedGameObject(cells[0].gameObject);
        }
    }

    //Funcionalidades internas
    private void UpdateCellsItems()
    {
        
        //Vacio la UI actual
        cells.ForEach(c => { c.CleanItemUI(); });
        CleanSelectInfo();
        //Relleno con la informacion sacada del inventario
        int i = 0;
        foreach (KeyValuePair<Item, int> item in inventory.ToList())
        {
            cells[i].SetItemUI(item.Key, item.Value);
            i++;
        }
    }

    public void SelectItem(InventoryCellController sC)
    {
        //Guardo la seleccion
        selectedCell = sC;
        //Actualizo la info de la interfaz
        SelectCellInfo(selectedCell.item);
    }

    private void SelectCellInfo(Item item)
    {
        nameUI.text = item.itemName;
        descriptionUI.text = item.description;

        useButtonUI.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = item.useButtonText;
        useButtonUI.interactable = item.usable;
        useButtonUI.gameObject.SetActive(true);
    }

    public void CleanSelectInfo()
    {
        if(nameUI)nameUI.text = "";
        if(descriptionUI) descriptionUI.text = "";
        if(useButtonUI) useButtonUI.interactable=false;
        if(useButtonUI) useButtonUI.gameObject.SetActive(false);
    }



}
