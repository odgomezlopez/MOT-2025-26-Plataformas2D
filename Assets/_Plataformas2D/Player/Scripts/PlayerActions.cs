using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerActions : MonoBehaviour
{
    //Referencias
    PlayerStatsComponent stats;

    //Variable para saber si el modificador esta activo o no
    [SerializeField] bool modificador = false;

    //SpawnPoints
    [Header("Spawn Points")]
    [SerializeField] public Transform spawnPoint;


    private void Awake()
    {
        stats = GameData.Instance.GetComponent<PlayerStatsComponent>();

        if(!spawnPoint) spawnPoint = transform;

        //Inicializamos los cooldown
        if (stats.stats.action1) stats.stats.action1.ResetCooldown();
        if (stats.stats.action1S) stats.stats.action1S.ResetCooldown();

        if (stats.stats.action2) stats.stats.action2.ResetCooldown();
        if (stats.stats.action2S) stats.stats.action2S.ResetCooldown();
    }

    private void Update()
    {
        if(stats.stats.action1) stats.stats.action1.UpdateCoolDown();
        if (stats.stats.action1S) stats.stats.action1S.UpdateCoolDown();

        if (stats.stats.action2) stats.stats.action2.UpdateCoolDown();
        if (stats.stats.action2S) stats.stats.action2S.UpdateCoolDown();
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
        if (action == null || action.IsOnCooldown()) return;

        //CoolDown
        action.StartCooldown();

        //Usar ataque
        action.Execute(gameObject);
    }
    #endregion

}
