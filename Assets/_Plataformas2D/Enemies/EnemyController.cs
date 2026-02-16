using System.Collections.Generic;
using UnityEngine;


public enum EnemyType
{
    Normal,
    Flying
}

public enum EnemyState
{
    Idle,
    Patrol
}

public class EnemyController : MonoBehaviour
{
    [Header("Basic Data")]
    StatsComponent stats;

    [SerializeField] EnemyState enemyState = EnemyState.Idle;
    [SerializeField] EnemyType enemyType = EnemyType.Normal;

    [Header("Idle state")]
    [SerializeField] float idleTime = 2f;
    [SerializeField] float idleTimer = 0f;

    [Header("Patrol state")]
    [SerializeField] List<Transform> patrolsPoints;
    List<Vector3> patrolsPositions;
    [SerializeField] int target = 0;

    //Components
    Rigidbody2D rb;
    //IGrounded2D grounded2D;

    void Start()
    {
        stats = GetComponent<StatsComponent>();
        rb = GetComponent<Rigidbody2D>();
        //grounded2D = GetComponentInChildren<IGrounded2D>();

        //Inicialize patrols positions to avoid target moving with us.
        target = 0;
        patrolsPositions = new();
        foreach (Transform point in patrolsPoints)
        {
            patrolsPositions.Add(point.position);
        }
        patrolsPositions.Add(transform.position);

        //Si es enemigo volador, desactivo la gravedad
        if(enemyType==EnemyType.Flying) rb.gravityScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        switch (enemyState) { 
            case EnemyState.Idle:
                UpdateIdle();
                break;
            case EnemyState.Patrol:
                UpdatePatrol();
                break;
        }
    }


    void UpdateIdle()
    {
        idleTimer += Time.deltaTime;
        if (idleTimer >= idleTime)
        {
            idleTimer = 0f;
            enemyState = EnemyState.Patrol;
        }
    }
    void UpdatePatrol()
    {
        if(Vector3.Distance(transform.position, patrolsPositions[target]) > 1.5f) // Si no hemos llegado, avanzamos hacia el objetivo
        {
            int targetDirection = patrolsPositions[target].x > transform.position.x ? 1 : -1;
            rb.linearVelocityX = Mathf.MoveTowards(rb.linearVelocityX, stats.stats.moveSpeed * targetDirection, stats.stats.moveSpeed); //TODO Va mejor en el FixedUpdate,
        }
        else //Si hemos llegado 
        { 
            rb.linearVelocityX = 0;
            target = (target + 1) % patrolsPositions.Count; //Pasamos al siguiente objetivo
            enemyState = EnemyState.Idle;
        }
    }
}
