using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformMoveInteraction : GenericInteraction
{
    [Header("Plataform Movement Parameters")]
    [SerializeField] float speed=5f;
    //[SerializeField] Transform origin;
    [SerializeField] Transform target;
    [SerializeField] float waitBetweenMovements = 1f;
    [SerializeField] bool loop = true;

    //Internal parameters
    List<Vector3> targets = new List<Vector3>();
    int currentTarget;

    private void Start()
    {
        targets.Add(transform.position);
        targets.Add(target.position);
        currentTarget = 1;
    }

    //Movimiento a la siguiente posición

    public override void Activate(GameObject activator)
    {
        StartCoroutine(MoveToNextPosition(activator));
    }

    private IEnumerator MoveToNextPosition(GameObject activator)
    {
        //Uso de herencia
        base.Activate(activator);

        //Activar
        IsEnable.Value = false;
        Vector3 targetPosition = targets[currentTarget];

        yield return new WaitForSeconds(waitBetweenMovements);

        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }

        transform.position = targetPosition;
        currentTarget = (currentTarget + 1) % targets.Count;

        yield return new WaitForSeconds(waitBetweenMovements);
        if(loop) IsEnable.Value = true;
    }
}