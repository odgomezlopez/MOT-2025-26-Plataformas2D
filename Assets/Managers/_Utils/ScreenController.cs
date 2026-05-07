using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ScreenController : MonoBehaviour
{
    [Header("Action map")]
    [SerializeField] protected string newActionMap;
    protected string oldActionMap;

    [Header("Actions")]
    [SerializeField] private InputActionReference showAction;
    [SerializeField] private InputActionReference hideAction;

    [Header("UI references")]
    [SerializeField] private Selectable firstSelectObject;

    [Header("Behaviour")]
    [SerializeField] private bool pauseGameWhenVisible = true;

    private PlayerInput _playerInput;
    private PlayerInput PlayerInput
    {
        get
        {
            if (_playerInput == null)
                _playerInput = FindFirstObjectByType<PlayerInput>();
            return _playerInput;
        }
    }

    private Canvas canvas;
    private bool isVisible;
    private bool eventLinked;

    protected virtual void Awake()
    {
        canvas = GetComponent<Canvas>();
        if (canvas == null)
            Debug.LogError($"[ScreenController] No Canvas on '{gameObject.name}'.", gameObject);
    }

    private void Start()
    {
        if (canvas != null) canvas.enabled = false;
    }

    protected virtual void OnEnable()
    {
        if (eventLinked) return;
        if (showAction) showAction.action.performed += ShowScreenConnector;
        if (hideAction) hideAction.action.performed += HideScreenConnector;
        eventLinked = true;
    }

    protected virtual void OnDisable()
    {
        if (showAction) showAction.action.performed -= ShowScreenConnector;
        if (hideAction) hideAction.action.performed -= HideScreenConnector;
        eventLinked = false;
    }

    public void ShowScreenConnector(InputAction.CallbackContext _ = default) => ShowScreen();
    public void HideScreenConnector(InputAction.CallbackContext _ = default) => HideScreen();

    public virtual void ShowScreen()
    {
        if (isVisible) return;
        if (canvas == null || PlayerInput == null)
        {
            Debug.LogWarning($"[ScreenController] ShowScreen called but references not ready on '{gameObject.name}'.");
            return;
        }

        isVisible = true;
        canvas.enabled = true;

        oldActionMap = PlayerInput.currentActionMap?.name;
        if (!string.IsNullOrEmpty(newActionMap))
            PlayerInput.SwitchCurrentActionMap(newActionMap);

        if (pauseGameWhenVisible) Time.timeScale = 0f;
        firstSelectObject?.Select();
    }

    public virtual void HideScreen()
    {
        if (!isVisible) return;
        if (canvas == null || PlayerInput == null) return;

        isVisible = false;
        canvas.enabled = false;

        if (!string.IsNullOrEmpty(oldActionMap))
            PlayerInput.SwitchCurrentActionMap(oldActionMap);

        if (pauseGameWhenVisible) Time.timeScale = 1f;
    }

    private void OnDestroy()
    {
        // Only restore time scale if we were the ones who paused it.
        if (isVisible && pauseGameWhenVisible) Time.timeScale = 1f;
    }
}