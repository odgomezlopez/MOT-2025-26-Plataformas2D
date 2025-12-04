using System;
using UnityEngine;

public class Trigger2DCheck : MonoBehaviour, ICheck, ICheckUI
{
    public bool IsMet => isMet;
    bool isMet;

    public event Action<bool> OnCheckValueChanged;

    [SerializeField] bool autoDisableRenderer = true;

    private void Awake()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (renderer && autoDisableRenderer) renderer.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<PlayerController>())
        {
            isMet = true;
            OnCheckValueChanged.Invoke(isMet);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<PlayerController>())
        {
            isMet = false;
            OnCheckValueChanged.Invoke(isMet);
        }
    }
}
