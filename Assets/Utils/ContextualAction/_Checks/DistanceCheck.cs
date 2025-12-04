using System;
using UnityEngine;

public class DistanceCheck : MonoBehaviour, ICheck, ICheckUI
{
    [Header("Range")]
    [SerializeField, Min(0f)] float activateDistance = 1.5f;
    [SerializeField] bool requiereReenter = true;

    [Header("References (optional)")]
    [SerializeField] Transform playerTransform;

    // ICheck implementation
    public bool IsMet => isMet;
    public event Action<bool> OnCheckValueChanged;

    bool isMet;

    void Start()
    {
        if (playerTransform == null)
            playerTransform = FindFirstObjectByType<PlayerController>()?.transform;

        UpdateValue(forceNotify: true);
    }

    void Update()
    {
        UpdateValue(!requiereReenter);
    }

    void UpdateValue(bool forceNotify = false)
    {
        bool newValue = ComputeIsMet();

        if (forceNotify || newValue != isMet)
        {
            isMet = newValue;
            OnCheckValueChanged?.Invoke(isMet);
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
        Gizmos.color = isMet ? Color.blue: Color.white;
        Gizmos.DrawWireSphere(transform.position, activateDistance);
    }
    #endregion
}
