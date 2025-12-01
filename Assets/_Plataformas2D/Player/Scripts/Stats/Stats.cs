using UnityEngine;

[System.Serializable]
public class Stats
{
    [Header("Character data")]
    public string characterName;

    [Header("Move parameters")]
    public float moveSpeed = 5f;
    [Range(1f, 5f)] public float runModifier = 1.5f;

    [Header("Aceleration parameters")]
    public float acceleration = 20f;
    public float deceleration = 40f;
    [Range(1f, 5f)] public float turnBoost = 1.5f;


    [Header("Jump parameters")]
    public float jumpForce = 10f;
    [Range(0f,1f)] public float airMomentum = 0.8f;

    [Range(0f, 0.2f)] public float jumpBuffer = 0.08f;
}
