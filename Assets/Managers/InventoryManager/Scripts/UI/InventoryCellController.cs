using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


//https://gamedev.stackexchange.com/questions/187065/how-to-trigger-onselect-on-a-button
//https://www.reddit.com/r/Unity3D/comments/q5xzkt/how_to_make_a_button_clickable_but_not_selectable/

public class InventoryCellController : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler
{
    public Item item;

    private Button boton;
    private Image background;
    private Image icon;

    private GameObject cantidadFondo;
    private TextMeshProUGUI cantidadText;
    InventoryUI inventoryUI;
    private void Awake()
    {
        boton =GetComponentInChildren<Button>();
        background = gameObject.GetComponent<Image>();
        icon = gameObject.transform.GetChild(0).GetComponent<Image>();

        cantidadText = GetComponentInChildren<TextMeshProUGUI>();
        cantidadFondo = gameObject.transform.Find("CantidadFondo").gameObject;

        inventoryUI = GetComponentInParent<InventoryUI>();
    }

    private void OnEnable()
    {
        boton.onClick.AddListener(OnSelect);
    }

    private void OnDisable()
    {
        boton.onClick.RemoveListener(OnSelect);
        OnDeselect();
    }

    #region Utilización de navegacion con el EventSystem
    public void OnSubmit(BaseEventData eventData) => OnSubmit();
    public void OnSubmit()
    {
        if (item != null) inventoryUI.OnUseButton();
    }

    public void OnSelect(BaseEventData eventData) => OnSelect();
    public void OnSelect()
    {
        background.color = Color.white;//TODO poner colores mediante materiales
        if (item != null) inventoryUI.SelectItem(this);
    }

    public void OnDeselect(BaseEventData eventData) => OnDeselect();

    public void OnDeselect()
    {
        if (item == null)
        {
            background.color = background.color = Color.Lerp(Color.gray, Color.black, 0.8f);
        }
        else
        {
            background.color = Color.Lerp(Color.gray, Color.white, 0.4f);
        }
        inventoryUI.CleanSelectInfo();
    }
    #endregion


    #region Gestion contenido celda desde InventoryUIController
    public void SetItemUI(Item elemento, int num)
    {
        item = elemento;

        icon.gameObject.SetActive(true);
        icon.sprite = elemento.sprite;

        cantidadFondo.SetActive(num > 1);
        cantidadText.text = num > 1 ? $"x{num}" : "";

        background.color = Color.white;

        boton.interactable = true;
        boton.enabled = true;

    }

    public void CleanItemUI()
    {
        item = null;
        icon.gameObject.SetActive(false);
        cantidadFondo.SetActive(false);

        cantidadText.text = "";

        background.color = background.color = Color.Lerp(Color.gray, Color.black, 0.8f);
        boton.interactable= false;
        boton.enabled = false;
    }
    #endregion
}
