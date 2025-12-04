using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Splines;
using UnityEngine.UIElements;

public class MovePlataform : MonoBehaviour, ISwitchable
{
    [Header("Plataform Movement Parameters")]
    [SerializeField] float speed=5f;
    [SerializeField] Transform target;
    //[SerializeField] float waitBetweenMovements = 1f;

    //Internal parameters
    List<Vector3> targets = new List<Vector3>();
    int currentTarget;

    private bool active;

    public event Action OnActionEnded;

    public bool IsActive => active;

    private void Start()
    {
        targets.Add(transform.position);
        targets.Add(target.position);
        currentTarget = 1;
    }

    //Movimiento a la siguiente posición

    public void Activate(GameObject activator)
    {
        StartCoroutine(MoveToNextPosition(activator));
    }

    private IEnumerator MoveToNextPosition(GameObject activator)
    {
        //Activar
        active = true;

        Vector3 targetPosition = targets[currentTarget];
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }

        transform.position = targetPosition;
        currentTarget = (currentTarget + 1) % targets.Count;

        //yield return new WaitForSeconds(waitBetweenMovements);
        active = false;
        OnActionEnded.Invoke();
    }
}