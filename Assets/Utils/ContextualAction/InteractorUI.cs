using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class InteractorUI : MonoBehaviour
{
    [Header("Availability behaviour")]
    [Tooltip("If false, the UI will stay visible even when the Interactor is inactive.")]
    [SerializeField] private bool hideWhenRequirementFail = true;

    [Header("Visual style")]
    [SerializeField] private Color availableTextColor = Color.white;
    [SerializeField] private Color unavailableTextColor = new Color(1f, 1f, 1f, 0.6f);

    [SerializeField] private Color availableBackgroundColor = Color.white;
    [SerializeField] private Color unavailableBackgroundColor = new Color(1f, 1f, 1f, 0.3f);

    //References
    Canvas canvas;

    Interactor interactor;

    IAction action;

    Image textBackground;
    TextMeshProUGUI actionText;
    PressKeyFromAction pressKeyFromAction;
    bool initialized = false;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        ShowHideCanvas(false);

        interactor = GetComponentInParent<Interactor>();
        action = interactor.Action;

        textBackground = GetComponentInChildren<Image>();
        actionText = GetComponentInChildren<TextMeshProUGUI>();
        pressKeyFromAction  = GetComponentInChildren<PressKeyFromAction>();

        //Si no hay iterator, distace check or action check se desactiva
        if(interactor == null) gameObject.SetActive(false);

        //Inicializamos los datos
        InitData();
    }

    public void InitData()
    {
        // Relaciono la acción del check con la que se muestra en UI
        if (interactor != null && actionText != null)
            actionText.text = action.ActionName;

        if (interactor != null && pressKeyFromAction != null)
            pressKeyFromAction.InputActionRef = interactor.InteractActionRef;

        initialized = true;
    }

    private void OnEnable()
    {
        interactor.OnIsActionEnable += ShowHideCanvas;
        interactor.OnChecksMet += ShowHideCanvas;

    }

    private void OnDisable()
    {
        interactor.OnIsActionEnable -= ShowHideCanvas;
        interactor.OnChecksMet -= ShowHideCanvas;
    }

    //TODO disable input when Interactor IsActive o AllMet. Si no está activo desactivar canvas. Si AllMet no se cumple cambiar el estilo. Añadir propiedades que hagan falta. También si es necesario añadir eventos a Interactor para detectar estos cambios.

    private void ShowHideCanvas(bool enable)
    {
        if (canvas == null) return;
        if (!initialized) return;

        // 1) Visibility:

        bool visible = interactor.Action.IsEnable && interactor.AllChecksMet  &&
                       (interactor.AllRequirementsMet || !hideWhenRequirementFail);

        canvas.enabled = visible;
        if (visible == false) return;


        // 2) Visual style if AllMet (CanInteract) is not fulfilled => change style
        Color bgColor = interactor.AllRequirementsMet ? availableBackgroundColor : unavailableBackgroundColor;
        Color txtColor = interactor.AllRequirementsMet ? availableTextColor : unavailableTextColor;

        if (textBackground != null)
            textBackground.color = bgColor;

        if (actionText != null)
            actionText.color = txtColor;
    }
}
