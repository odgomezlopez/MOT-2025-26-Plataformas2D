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
    [SerializeField] public GameObject ataque1Prefab;
    [SerializeField] public float ataque1Damage = 1f;
    [SerializeField] public float ataque1AnimationSpeed = 1f;
    [SerializeField] public float ataque1Speed = 1f;
    [SerializeField] public float ataque1CoolDown = 0.5f;
    [SerializeField] public bool ataque1DestroyOnDamage = true;
    [SerializeField] public float ataque1TemporalInv = 0.5f;
    [SerializeField] public float ataque1MaxLife = 2f; 
}
