using UnityEngine;

public class Action : ScriptableObject
{
    [Header("Action Info")]
    public string actionName = "";

    public float cooldown = 0f;
    public float cooldownTimer = 0f;

    // Method to execute the action
    public virtual void Execute(GameObject usedBy)
    {
        //Check if the action can be executed (e.g., cooldown)
        if (IsOnCooldown()) { 
            Debug.Log($"Action {actionName} is on cooldown. Time remaining: {cooldownTimer} seconds.");
            return; 
        }
        
        Debug.Log($"Executing action: {actionName}");
    }

    #region Cooldown management
    // Method to update the cooldown timer
    public void StartCooldown()
    {
        cooldownTimer = cooldown;
    }

    public void ResetCooldown()
    {
        cooldownTimer = 0f;
    }

    public bool IsOnCooldown()
    {
        return cooldownTimer > 0f;
    }

    public void UpdateCoolDown()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer < 0) cooldownTimer = 0;
        }
    }
    #endregion
}
