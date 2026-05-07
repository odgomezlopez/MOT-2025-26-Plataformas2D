using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryUI : ScreenController
{
    [Header("Inventory actions")]
    [SerializeField] InputActionReference useAction;

    List<InventoryCellController> cells;
    private InventoryCellController selectedCell;

    private TextMeshProUGUI nameUI;
    private TextMeshProUGUI descriptionUI;
    private Button useButtonUI;
    InventoryManager inventory => InventoryManager.Instance;

    protected override void Awake()
    {
        base.Awake(); // Ensures canvas = GetComponent<Canvas>() runs

        var info = gameObject.transform.GetChild(0).Find("Info");

        nameUI = info?.Find("Name")?.GetComponent<TextMeshProUGUI>();
        descriptionUI = info?.Find("Description")?.GetComponent<TextMeshProUGUI>();
        useButtonUI = info?.Find("UseButton")?.GetComponent<Button>();

        if (nameUI == null) Debug.LogError("[InventoryUI] 'Name' TMP not found under Info.", gameObject);
        if (descriptionUI == null) Debug.LogError("[InventoryUI] 'Description' TMP not found under Info.", gameObject);
        if (useButtonUI == null) Debug.LogError("[InventoryUI] 'UseButton' not found under Info.", gameObject);

        cells = new List<InventoryCellController>(GetComponentsInChildren<InventoryCellController>());
        useButtonUI?.onClick.AddListener(OnUseButton);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (useAction) useAction.action.performed += OnUseButtonConnector;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (useAction) useAction.action.performed -= OnUseButtonConnector;
    }

    public override void ShowScreen()
    {
        base.ShowScreen();

        if (cells.Count == 0)
            cells = new List<InventoryCellController>(GetComponentsInChildren<InventoryCellController>());

        UpdateCellsItems();

        if (inventory.Count != 0)
        {
            SelectItem(cells[0]);
            EventSystem.current.SetSelectedGameObject(cells[0].gameObject);
        }
    }

    public override void HideScreen()
    {
        base.HideScreen();
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void OnUseButtonConnector(InputAction.CallbackContext context = default)
    {
        OnUseButton();
    }

    public void OnUseButton()
    {
        if (selectedCell == null || selectedCell.item == null) return;
        if (!selectedCell.item.usable) return;

        var item = selectedCell.item;
        item.Use();

        if (item.deleteOnUse) inventory.RemoveItemFromInventory(item);

        UpdateCellsItems();

        if (inventory.Count != 0)
        {
            if (selectedCell.item != null && selectedCell.item.itemName == item.itemName)
                return; // Item still present, keep current selection

            SelectItem(cells[0]);
            EventSystem.current.SetSelectedGameObject(cells[0].gameObject);
        }
        else
        {
            // Inventory now empty — clear stale selection
            selectedCell = null;
            CleanSelectInfo();
        }
    }

    private void UpdateCellsItems()
    {
        cells.ForEach(c => c.CleanItemUI());
        CleanSelectInfo();

        int i = 0;
        foreach (KeyValuePair<Item, int> item in inventory.GetSnapshot())
        {
            cells[i].SetItemUI(item.Key, item.Value);
            i++;
        }
    }

    public void SelectItem(InventoryCellController sC)
    {
        selectedCell = sC;
        SelectCellInfo(selectedCell.item);
    }

    private void SelectCellInfo(Item item)
    {
        nameUI.text = item.itemName;
        descriptionUI.text = item.description;
        useButtonUI.GetComponentInChildren<TextMeshProUGUI>().text = item.useButtonText;
        useButtonUI.interactable = item.usable;
        useButtonUI.gameObject.SetActive(true);
    }

    public void CleanSelectInfo()
    {
        if (nameUI) nameUI.text = "";
        if (descriptionUI) descriptionUI.text = "";
        if (useButtonUI) useButtonUI.interactable = false;
        if (useButtonUI) useButtonUI.gameObject.SetActive(false);
    }
}