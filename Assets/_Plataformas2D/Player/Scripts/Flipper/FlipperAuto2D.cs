using System;
using UnityEngine;

public class FlipperAuto2D : MonoBehaviour
{
    Flipper2D flipper;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        flipper = GetComponentInParent<Flipper2D>();
    }

    private void OnEnable()
    {
        flipper.OnFlipChanged += Flipper_OnFlipChanged;
    }

    private void OnDisable()
    {
        flipper.OnFlipChanged -= Flipper_OnFlipChanged;
    }

    private void Flipper_OnFlipChanged(bool flip)
    {
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * (flip ? 1 : -1), transform.localScale.y, transform.localScale.z);
    }

}
