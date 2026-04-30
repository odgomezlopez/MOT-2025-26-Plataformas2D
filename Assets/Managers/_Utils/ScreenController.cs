using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;
using UnityEngine.UI;

public class ScreenController : MonoBehaviour
{
    //Internal references
    Canvas canvas;
    PlayerInput playerInput;

    [Header("Action map")]
    [SerializeField] string newActionMap;
    string oldActionMap;


    [Header("Actions")]
    [SerializeField] InputActionReference showAction;
    [SerializeField] InputActionReference hideAction;

    [Header("UI references")]
    [SerializeField] Selectable firstSelectObject;

    //Guardamo el fixedDetalTime para poder pausar bien
    private float fixedDeltaTime;

    void Awake()
    {
        this.fixedDeltaTime = Time.fixedDeltaTime;
    }

    void Start()
    {
        canvas = GetComponent<Canvas>();
        playerInput = FindObjectOfType<PlayerInput>();
        canvas.enabled = false;
    }

    public virtual void OnEnable()
    {
        if(showAction)showAction.action.performed += ShowScreenConnector;
        if (hideAction) hideAction.action.performed += HideScreenConnector;

    }

    public virtual void OnDisable()
    {
        if (showAction) showAction.action.performed -= ShowScreenConnector;
        if (hideAction) hideAction.action.performed -= HideScreenConnector;
    }



    public virtual void ShowScreenConnector(InputAction.CallbackContext context = default)
    {
        ShowScreen();
    }


    public virtual void ShowScreen()
    {
        canvas.enabled = true;
        oldActionMap = playerInput.currentActionMap.name;
        playerInput.SwitchCurrentActionMap(newActionMap);

        //Pausar
        ChangeTimeScale(0f);
        firstSelectObject?.Select();
    }

    public void HideScreenConnector(InputAction.CallbackContext context = default)
    {
        HideScreen();
    }
    
    public virtual void HideScreen()
    {
        canvas.enabled = false;
        playerInput.SwitchCurrentActionMap(oldActionMap);

        //Despausar
        ChangeTimeScale(1f);

    }
    private void OnDestroy()
    {
        ChangeTimeScale(1f);
    }

    private void ChangeTimeScale(float newScale)
    {
        Time.timeScale = newScale;
    }
}
