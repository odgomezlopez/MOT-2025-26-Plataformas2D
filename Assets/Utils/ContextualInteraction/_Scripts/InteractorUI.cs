using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(100)] // after Interactor most of the time
[RequireComponent(typeof(Canvas))]
public class InteractorUI : MonoBehaviour
{
    [Header("Availability behaviour")]
    [Tooltip("If false, the UI will stay visible even when the Interactor is inactive.")]
    [SerializeField] private bool hideWhenRequirementFail = false;

    [Header("Visual style")]
    [SerializeField] private Color availableTextColor = Color.black;
    [SerializeField] private Color unavailableTextColor = new Color(1f, 1f, 1f, 0.6f);

    [SerializeField] private Color availableBackgroundColor = Color.white;
    [SerializeField] private Color unavailableBackgroundColor = new Color(1f, 0f, 0f, 0.3f);

    //References
    Canvas canvas;

    Interactor interactor;

    IInteraction action;

    Image textBackground;
    TextMeshProUGUI actionText;
    PressKeyFromAction pressKeyFromAction;
    bool initialized = false;

    private void Awake()
    {
        //Get Canvas and disable (until ready)
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;

        //Get Interactor
        interactor = GetComponentInParent<Interactor>();
        action = interactor.Action;

        //Si no hay iterator, distace check or action check se desactiva
        if (interactor == null) { gameObject.SetActive(false); return; }

        //Get UI elements
        textBackground = GetComponentInChildren<Image>();
        actionText = GetComponentInChildren<TextMeshProUGUI>();
        pressKeyFromAction  = GetComponentInChildren<PressKeyFromAction>();


        //Inicializamos los datos
        InitData();
    }

    public void InitData()
    {
        if(interactor.InteractActionRef == null) return;
        

        // Relaciono la acción del check con la que se muestra en UI
        if (actionText != null)
            actionText.text = action != null ? action.ActionName : "<No Action>";

        if (pressKeyFromAction != null)
            pressKeyFromAction.InputActionRef = interactor.InteractActionRef;

        initialized = true;
        ShowHideCanvas(false);
    }

    private void OnEnable()
    {
        if (interactor == null) return;

        // These can be null if action missing. Guard them.
        if (interactor.IsActionEnable != null)
            interactor.IsActionEnable.OnValueChanged += ShowHideCanvas;

        if (interactor.AllChecksMet != null)
            interactor.AllChecksMet.OnValueChanged += ShowHideCanvas;

        if (interactor.AllRequiermentsMet != null)
            interactor.AllRequiermentsMet.OnValueChanged += ShowHideCanvas; // you were missing this

    }

    private void OnDisable()
    {
        if (interactor == null) return;

        if (interactor.IsActionEnable != null)
            interactor.IsActionEnable.OnValueChanged -= ShowHideCanvas;

        if (interactor.AllChecksMet != null)
            interactor.AllChecksMet.OnValueChanged -= ShowHideCanvas;

        if (interactor.AllRequiermentsMet != null)
            interactor.AllRequiermentsMet.OnValueChanged -= ShowHideCanvas;
    }


    private void ShowHideCanvas(bool enable)
    {
        if (canvas == null) return;
        if (!initialized) return;

        // 1) Visibility:

        bool visible = interactor.IsActionEnable.Value && interactor.AllChecksMet.Value  &&
                       (interactor.AllRequiermentsMet.Value || !hideWhenRequirementFail);

        canvas.enabled = visible;
        if (visible == false) return;


        // 2) Visual style if AllMet (CanInteract) is not fulfilled => change style
        Color bgColor = interactor.AllRequiermentsMet.Value ? availableBackgroundColor : unavailableBackgroundColor;
        Color txtColor = interactor.AllRequiermentsMet.Value ? availableTextColor : unavailableTextColor;

        if (textBackground != null)
            textBackground.color = bgColor;

        if (actionText != null)
            actionText.color = txtColor;
    }
}
