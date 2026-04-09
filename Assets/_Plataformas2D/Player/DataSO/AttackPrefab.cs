using UnityEngine;

[CreateAssetMenu(fileName = "New AttackPrefab", menuName = "Action/AttackPrefab", order = 1)]
public class AttackPrefab : Attack
{
    [Header("AttackPrefab")]
    [SerializeField] public GameObject prefab;
    [SerializeField] public Color color = Color.white;
    [SerializeField] public RuntimeAnimatorController attackAnimator;

    [SerializeField] public float animationSpeed = 1f;
    [SerializeField] public float speed = 1f;
    [SerializeField] public bool destroyOnDamage = true;
    [SerializeField] public float maxLife = 2f;

    [Header("SpawnPoint")]
    [SerializeField] public bool usedByCenter = true;
    [SerializeField] public Vector2 offset = Vector2.zero;


    public override void Execute(GameObject usedBy)
    {
        base.Execute(usedBy);

        if (prefab == null)
        { 
            Debug.LogWarning("No prefab assigned for this attack.");
            return;
        }

        //Inicializaciones
        StatsComponent stats = usedBy.GetComponent<StatsComponent>();
        Flipper2D flipper = usedBy.GetComponentInChildren<Flipper2D>();
        PlayerActions playerActions = usedBy.GetComponent<PlayerActions>();

        Debug.Log("Action 1 Normal");


        //Calculos generales
        int direction = flipper.isFacingRight() ? 1 : -1;

        //Calculo del punto de spawn del ataque, dependiendo de si queremos que se spawnee desde el centro del jugador o desde un punto personalizado.
        Vector2 spawnPoint;
        if (usedByCenter) spawnPoint = (Vector2)usedBy.transform.position + offset;
        else spawnPoint = (Vector2)playerActions.spawnPoint.position + offset;

        //Instanciar Prefab de ataque
        GameObject g = Instantiate(prefab, spawnPoint, Quaternion.identity); // CAMBIAR usedBy.transform por spawnPoint1
        g.transform.localScale = new Vector3(g.transform.localScale.x * direction, g.transform.localScale.y, g.transform.localScale.z);//Giramos el ataque.

        //Cambiar el color del ataque segun lo que tengamos en el scriptable object de stats.
        SpriteRenderer spriteRenderer = g.GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.color = color;

        if (attackAnimator)
        {
            var ani = spriteRenderer.GetComponent<Animator>();
            if(ani) ani.runtimeAnimatorController = attackAnimator;
        }

        //Asegurar layer del ataque
        g.layer = usedBy.layer;

        //Cambiar los valores del script DoDamage del ataque para que haga el daño correcto, y que el origen del daño sea el jugador.
        DoDamage doDamage = g.GetComponentInChildren<DoDamage>();
        if (doDamage != null)
        {
            doDamage.attParent = g;//Asignamos el origen del ataque.

            doDamage.damage = damage; //Asignamos el daño del ataque segun lo que tengamos en el scriptable object de stats.
            doDamage.destroyOnDamage = destroyOnDamage; //Si qeremos que se destruya al hacer daño o no.
            doDamage.gameObject.layer = usedBy.layer;//Asegurar layer del ataque
        }

        //Modificar velocidad del ataque, animacion
        Animator animator = g.GetComponentInChildren<Animator>();
        if (animator) animator.speed = animationSpeed;

        //Invulnerabilidad durante ataque
        stats.TemporalInvulnerability(temporalInv, false, false);

        //Hacer que los ataques a distancia se muevan hacia adelante (con RB o Script MoveTowards)
        MoveFoward moveFoward = g.GetComponentInChildren<MoveFoward>();
        if (moveFoward)
        {
            moveFoward.direction = new Vector2(direction, 0);
            moveFoward.speed = speed;
        }
        //TODO Efectos de sonido y visuales

        //TODO Posibilidad cambiar animacion del zorrito

        //Vida maxima
        Destroy(g, maxLife);
    }
}
