using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class InteractorUI : MonoBehaviour
{
    [Header("Availability behaviour")]
    [Tooltip("If false, the UI will stay visible even when the Interactor is inactive.")]
    [SerializeField] private bool hideWhenInteractorInactive = true;

    [Tooltip("If true, input check will be disabled when the Interactor can't be used.")]
    [SerializeField] private bool disableInputWhenUnavailable = true;

    [Header("Visual style")]
    [SerializeField] private Color availableTextColor = Color.white;
    [SerializeField] private Color unavailableTextColor = new Color(1f, 1f, 1f, 0.6f);

    [SerializeField] private Color availableBackgroundColor = Color.white;
    [SerializeField] private Color unavailableBackgroundColor = new Color(1f, 1f, 1f, 0.3f);

    //References
    Canvas canvas;

    Interactor interactor;
    ICheckUI uiCheck;
    InputActionCheck actionCheck;

    Image textBackground;
    TextMeshProUGUI actionText;
    PressKeyFromAction pressKeyFromAction;
    bool initialized = false;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        ShowHideCanvas(false);

        interactor = GetComponentInParent<Interactor>();
        uiCheck = interactor?.GetComponentInChildren<ICheckUI>();//If there are multiple UI checks, only the first one is considered
        actionCheck = interactor?.GetComponentInChildren<InputActionCheck>();

        textBackground = GetComponentInChildren<Image>();
        actionText = GetComponentInChildren<TextMeshProUGUI>();
        pressKeyFromAction  = GetComponentInChildren<PressKeyFromAction>();

        //Si no hay iterator, distace check or action check se desactiva
        if(interactor == null || uiCheck == null || actionCheck == null) gameObject.SetActive(false);

        //Inicializamos los datos
        InitData();
    }

    public void InitData()
    {
        // Relaciono la acción del check con la que se muestra en UI
        if (interactor != null && actionText != null)
            actionText.text = interactor.InteractionName;

        if (pressKeyFromAction != null && actionCheck != null)
            pressKeyFromAction.InputActionRef = actionCheck.InteractActionRef;

        initialized = true;
    }

    private void OnEnable()
    {
        uiCheck.OnCheckValueChanged += ShowHideCanvas;
        interactor.OnBusy += ShowHideCanvas;
        interactor.OnAllMet += ShowHideCanvas;

    }

    private void OnDisable()
    {
        uiCheck.OnCheckValueChanged -= ShowHideCanvas;
        interactor.OnBusy -= ShowHideCanvas;
        interactor.OnAllMet -= ShowHideCanvas;
    }

    //TODO disable input when Interactor IsActive o AllMet. Si no está activo desactivar canvas. Si AllMet no se cumple cambiar el estilo. Añadir propiedades que hagan falta. También si es necesario añadir eventos a Interactor para detectar estos cambios.

    private void ShowHideCanvas(bool enable)
    {
        if (canvas == null) return;
        if (!initialized) return;

        // 1) Visibility:
        //    - must be "in range" according to ICheckUI
        //    - and (optionally) the Interactor must be active
        bool visible = uiCheck.IsMet &&
                       (!hideWhenInteractorInactive || !interactor.Busy);

        canvas.enabled = visible;

        // 2) Input availability:
        //    Disable input when Interactor is NOT active or when conditions are NOT met.
        //    (Matches your TODO comment.)
        bool inputAllowed = visible &&
                            !interactor.Busy &&
                            interactor.AllMet;

        // 3) Visual style if AllMet (CanInteract) is not fulfilled => change style
        Color bgColor = interactor.AllMet ? availableBackgroundColor : unavailableBackgroundColor;
        Color txtColor = interactor.AllMet ? availableTextColor : unavailableTextColor;

        if (textBackground != null)
            textBackground.color = bgColor;

        if (actionText != null)
            actionText.color = txtColor;
    }
}
