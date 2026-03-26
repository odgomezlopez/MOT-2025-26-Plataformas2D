using UnityEngine;

public class Attack : Action
{
    [Header("Attack Data")]
    [SerializeField] public float damage = 1f;
    [SerializeField] public float temporalInv = 0.5f;

}
