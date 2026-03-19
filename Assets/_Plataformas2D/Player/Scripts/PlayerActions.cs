using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{
    //Referencias
    StatsComponent stats;
    Flipper2D flipper;

    //Variable para saber si el modificador esta activo o no
    [SerializeField] bool modificador = false;

    //SpawnPoints
    [Header("Spawn Points")]
    [SerializeField] Transform spawnPoint1;

    //ColdDowns
    [Header("CoolDowns")]
    [SerializeField] float cooldownAtk1Normal=0;
    [SerializeField] float cooldownAtk1Sp = 0;
    [SerializeField] float cooldownAtk2Normal = 0;
    [SerializeField] float cooldownAtk2Sp = 0;

    private void Awake()
    {
        stats = GetComponent<StatsComponent>();
        flipper = GetComponent<Flipper2D>();

        if(!spawnPoint1) spawnPoint1 = transform;

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
        if(modificador) Debug.Log("Action 1 Especial");
        else Action1Normal(); 
    }

    public void Action2(InputAction.CallbackContext context = default) {
        if (modificador) Debug.Log("Action 2 Special");
        else Debug.Log("Action 2 Normal");
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
    private void Action1Normal()
    {
        Debug.Log("Action 1 Normal");
        //CoolDown
        if (cooldownAtk1Normal > 0) return;
        cooldownAtk1Normal = stats.stats.ataque1CoolDown;

        //Calculos generales
        int direction = flipper.isFacingRight() ? 1 : -1;

        //Instanciar Prefab de ataque
        GameObject g = Instantiate(stats.stats.ataque1Prefab, spawnPoint1.position, Quaternion.identity); //,spawnPoint1
        g.transform.localScale = new Vector3(g.transform.localScale.x * direction, g.transform.localScale.y, g.transform.localScale.z);//Giramos el ataque.

        //Asegurar layer del ataque
        g.layer = gameObject.layer;

        //Cambiar los valores del script DoDamage del ataque para que haga el daño correcto, y que el origen del daño sea el jugador.
         DoDamage doDamage = g.GetComponentInChildren<DoDamage>();
         if(doDamage != null)
         {
            doDamage.attParent = g;//Asignamos el origen del ataque.

            doDamage.damage = stats.stats.ataque1Damage; //Asignamos el daño del ataque segun lo que tengamos en el scriptable object de stats.
            doDamage.destroyOnDamage = stats.stats.ataque1DestroyOnDamage; //Si qeremos que se destruya al hacer daño o no.
            doDamage.gameObject.layer = gameObject.layer;//Asegurar layer del ataque
        }

        //Modificar velocidad del ataque, animacion
        Animator animator = g.GetComponentInChildren<Animator>();
        if (animator) animator.speed = stats.stats.ataque1AnimationSpeed;

        //Invulnerabilidad durante ataque
        stats.TemporalInvulnerability(stats.stats.ataque1TemporalInv,false,false);

        //Hacer que los ataques a distancia se muevan hacia adelante (con RB o Script MoveTowards)
        MoveFoward moveFoward = g.GetComponentInChildren<MoveFoward>();
        if (moveFoward)
        {
            moveFoward.direction = new Vector2(direction, 0);
            moveFoward.speed = stats.stats.ataque1Speed;
        }
        //TODO Efectos de sonido y visuales

        //TODO Posibilidad cambiar animacion del zorrito

        //Vida maxima
        Destroy(g, stats.stats.ataque1MaxLife);
    }
    #endregion

}
