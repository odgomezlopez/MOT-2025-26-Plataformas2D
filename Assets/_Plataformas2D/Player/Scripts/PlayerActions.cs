using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerActions : MonoBehaviour
{
    //Referencias
    StatsComponent stats;

    //Variable para saber si el modificador esta activo o no
    [SerializeField] bool modificador = false;

    //SpawnPoints
    [Header("Spawn Points")]
    [SerializeField] public Transform spawnPoint;

    //ColdDowns
    [Header("CoolDowns")]
    [SerializeField] float cooldownAtk1Normal=0;
    [SerializeField] float cooldownAtk1Sp = 0;
    [SerializeField] float cooldownAtk2Normal = 0;
    [SerializeField] float cooldownAtk2Sp = 0;

    private void Awake()
    {
        stats = GetComponent<StatsComponent>();

        if(!spawnPoint) spawnPoint = transform;

        //Inicializamos los cooldown
        cooldownAtk1Normal = 0; 
        cooldownAtk1Sp = 0;
        cooldownAtk2Normal= 0;
        cooldownAtk2Sp = 0;
    }

    private void Update()
    {
        UpdateCoolDown(ref cooldownAtk1Normal);
        UpdateCoolDown(ref cooldownAtk1Sp);
        UpdateCoolDown(ref cooldownAtk2Normal);
        UpdateCoolDown(ref cooldownAtk2Sp);
    }

    private void UpdateCoolDown(ref float coolDown)
    {
        if (coolDown > 0)
        {
            coolDown -= Time.deltaTime;
            if (coolDown < 0) coolDown = 0;
        }
    }


    #region Metodos de conexión
    public void Action1(InputAction.CallbackContext context = default)
    {
        if(modificador) UseAction(stats.stats.action1S);
        else UseAction(stats.stats.action1);

        //if (stats.action1Up & input.y > 0.5) stats.action1Up.Use(gameObject);
        //else if (stats.action1Down & input.y < -0.5) stats.action1Down.Use(gameObject);
        //else stats.action1.Use(gameObject);
    }

    public void Action2(InputAction.CallbackContext context = default) {
        if (modificador) UseAction(stats.stats.action2S);
        else UseAction(stats.stats.action2);
    }

    public void ActivarModifiador(InputAction.CallbackContext context = default)
    {
        modificador = true;
    }

    public void DesactivarModifiador(InputAction.CallbackContext context = default)
    {
        modificador = false;
    }
    #endregion

    #region Metodos privados
    private void UseAction(Action action)
    {
        //Comprobacaiones previas
        if (action == null) return;

        //CoolDown
        if (cooldownAtk1Normal > 0) return;
        cooldownAtk1Normal = action.cooldown;

        //Usar ataque
        action.Execute(gameObject);
    }
    #endregion

}
