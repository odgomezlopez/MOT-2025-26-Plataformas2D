using UnityEngine;

public class Action : ScriptableObject
{
    [Header("Action Info")]
    public string actionName = "";
    public float cooldown = 0f;

    public virtual void Execute(GameObject usedBy)
    {
        Debug.Log($"Executing action: {actionName}");
    }
}
