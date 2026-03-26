using UnityEngine;

[System.Serializable]
public class Stats
{
    [Header("Character data")]
    public string characterName;

    [Header("Indicators")]
    public ObservableRangedFloat HP;

    [Header("Invulnerabilidad")]
    public bool invulnerability = false;
    public float invulnerabilityDuration = 0.5f;
    public bool invulnerabilityChangeColor = true;
    public Color invulnerabilityColor = Color.red;

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

    [Header("Attacks parameters")]
    [SerializeField] public Action action1;
    [SerializeField] public Action action1S;
    [SerializeField] public Action action2;
    [SerializeField] public Action action2S;
}
