using System;
using UnityEngine;

public class Trigger2DCheck : MonoBehaviour, ICheck
{

    [SerializeField] private ObservableValue<bool> isMet = new();
    public ObservableValue<bool> IsMet => isMet;

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
            IsMet.Value = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<PlayerController>())
        {
            IsMet.Value = false;
        }
    }
}
