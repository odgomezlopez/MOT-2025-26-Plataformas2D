using System;
using UnityEngine;

public class DistanceCheck : MonoBehaviour, ICheck
{
    [Header("Range")]
    [SerializeField, Min(0f)] float activateDistance = 1.5f;
    [SerializeField] bool requiereReenter = true;

    [Header("References (optional)")]
    [SerializeField] Transform playerTransform;

    [Header("Performance")]
    [SerializeField] float frameRate = 5;

    // ICheck implementation

    [SerializeField] private ObservableValue<bool> isMet = new();
    public ObservableValue<bool> IsMet => isMet;

    void Start()
    {
        if (playerTransform == null)
            playerTransform = FindFirstObjectByType<PlayerController>()?.transform;

        UpdateValue(forceNotify: true);
    }


    void Update()
    {
        if(Time.frameCount % frameRate == 0)UpdateValue(!requiereReenter);
    }

    void UpdateValue(bool forceNotify = false)
    {
        bool newValue = ComputeIsMet();

        if (forceNotify || newValue != IsMet.Value)
        {
            IsMet.Value = newValue;
        }
    }

    bool ComputeIsMet()
    {
        if (!playerTransform) return false;

        return Vector2.Distance(transform.position, playerTransform.position) < activateDistance;
    }

    //private void OnDisable()
    //{
    //    // Optional: when disabled, treat as "not met"
    //    if (isMet)
    //    {
    //        isMet = false;
    //        OnCheckValueChanged?.Invoke(this, isMet);
    //    }
    //}

    #region Editor's utilities
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = IsMet.Value ? Color.blue: Color.white;
        Gizmos.DrawWireSphere(transform.position, activateDistance);
    }
    #endregion
}
