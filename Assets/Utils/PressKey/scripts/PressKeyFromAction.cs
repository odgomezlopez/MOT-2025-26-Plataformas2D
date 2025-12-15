
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.Switch;
using UnityEngine.InputSystem.XInput;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class PressKeyFromAction : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference inputAction;
    [Tooltip("Si se deja vacío, se usará el primer PlayerInput encontrado en la escena.")]
    [SerializeField] private PlayerInput playerInput;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI textComponent;
    [Tooltip("Imagen opcional de fondo (por ejemplo, un keycap redondo detrás del símbolo).")]
    [SerializeField] private Image backgroundImage;
    [Tooltip("Texto a mostrar cuando no hay binding activo para el esquema actual.")]
    [SerializeField] private string noBindingText = "—";

    // Control scheme actual
    private string activeControlScheme;

    //Getters/Setters
    public InputActionReference InputActionRef { 
        get => inputAction; 
        set {
            inputAction = value;
            UpdateDisplay();
        }
    }

    #region Life Cycle
    private void Awake()
    {
        Init();

        // Initialize text with current binding
        UpdateDisplay();
    }


    private void OnEnable()
    {
        Init();

        if (playerInput != null)
            playerInput.onControlsChanged += OnControlsChanged;
    }

    private void OnDisable()
    {
        if (playerInput != null)
            playerInput.onControlsChanged -= OnControlsChanged;
    }
    #endregion

    #region Métodos públicos extra

    /// <summary>
    /// Fuerza un refresco de la UI (útil tras un rebinding en runtime).
    /// </summary>
    public void Refresh() => UpdateDisplay();

    /// <summary>
    /// Devuelve el texto/símbolo actualmente mostrado.
    /// </summary>
    public string GetCurrentDisplayText() => textComponent ? textComponent.text : string.Empty;

    public void ChangeColors(Color textColor, Color backgroundColor)
    {
        if (textComponent)
            textComponent.color = textColor;
        if (backgroundImage)
            backgroundImage.color = backgroundColor;
    }

    #endregion

    #region Metodos privados
    private void Init()
    {
        if (!textComponent)
            textComponent = GetComponent<TextMeshProUGUI>();
        if(!backgroundImage)
            backgroundImage = GetComponentInChildren<Image>();


        if (!playerInput)
            playerInput = FindFirstObjectByType<PlayerInput>();

        if (!playerInput)
        {
            Debug.LogWarning($"{nameof(PressKeyFromAction)}: No se ha encontrado ningún PlayerInput en la escena.", this);
            return;
        }

        activeControlScheme = playerInput.currentControlScheme;
    }

    private void OnControlsChanged(PlayerInput obj)
    {
        if (!playerInput) return;

        if (activeControlScheme != playerInput.currentControlScheme)
        {
            activeControlScheme = playerInput.currentControlScheme;
            UpdateDisplay();
        }
    }

    private void UpdateDisplay()
    {
        if (InputActionRef?.action == null) return;

        string key = GetDisplayString();
        textComponent.SetText(key);
    }

    private string GetDisplayString()
    {
        if (playerInput == null)
        {
            Init();
            if (playerInput == null)
                return noBindingText;
        }

        if (InputActionRef == null || InputActionRef.action == null)
            return noBindingText;

        string controlScheme = playerInput.currentControlScheme;
        if (string.IsNullOrEmpty(controlScheme))
            return noBindingText;

        controlScheme = controlScheme.ToLowerInvariant();

        // Buscar binding activo para el scheme
        InputBinding activeBinding = InputActionRef.action.bindings
            .FirstOrDefault(binding =>
                !string.IsNullOrEmpty(binding.groups) &&
                binding.groups
                    .Split(';')
                    .Any(scheme => scheme.Trim().ToLowerInvariant() == controlScheme));

        if (activeBinding == default)
            return noBindingText;

        // Texto que saca Unity (ej: "A", "Cross", "Space", "Left Arrow"...)
        string display = activeBinding.ToDisplayString(
            InputBinding.DisplayStringOptions.DontIncludeInteractions);

        // Si es gamepad, intento afinar a xbox / playstation / switch
        string schemeForLookup = controlScheme == "gamepad"
            ? GetDeviceType()
            : controlScheme;

        // 1º intento: usar display string
        if (KeyToSymbol.TryGetSymbol(schemeForLookup, display, out string symbol))
            return symbol;

        // 2º intento: usar el nombre técnico del control (effectivePath)
        string controlName = GetControlNameFromBinding(activeBinding);
        if (KeyToSymbol.TryGetSymbol(schemeForLookup, controlName, out string symbolFromPath))
            return symbolFromPath;

        // Fallback: texto normal
        return display;
    }

    private static string GetControlNameFromBinding(InputBinding binding)
    {
        if (string.IsNullOrEmpty(binding.effectivePath))
            return string.Empty;

        int lastSlash = binding.effectivePath.LastIndexOf('/');
        return lastSlash >= 0 && lastSlash < binding.effectivePath.Length - 1
            ? binding.effectivePath[(lastSlash + 1)..]
            : binding.effectivePath;
    }

    private string GetDeviceType()
    {
        if (playerInput == null)
            return "gamepad";

        string controlScheme = playerInput.currentControlScheme.ToLowerInvariant();

        if (controlScheme == "keyboard&mouse")
            return "keyboard&mouse";

        if (controlScheme == "gamepad" && Gamepad.current != null)
        {
            return Gamepad.current switch
            {
                DualShockGamepad => "playstation",
                XInputController => "xbox",
                SwitchProControllerHID => "switch",
                _ => "gamepad" // Generic gamepad if type is unknown
            };
        }

        //TODO Añadir else if para XR, etc. 
        return controlScheme;
    }
    #endregion
}
