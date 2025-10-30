using System.Collections;
using UnityEngine;

public class TriggerAnimation : MonoBehaviour
{

    [SerializeField] Animator animator;
    public string animationTriggerName = "Activada";


    [SerializeField] SpriteRenderer sprite; //Asignar a mano en el editor si se quiere desactivar
    [SerializeField, Range(0f,5f)] float waitTime = 0f; 

    private void Start()
    {
        if (animator == null) animator = GetComponentInChildren<Animator>();
        if (animator == null) Debug.LogError("TriggerAnimation: Animator not attached/found");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(TriggerAndDisable());
        }
    }

    private IEnumerator TriggerAndDisable()
    {
        animator.SetTrigger(animationTriggerName);
        if (sprite)
        {
            yield return new WaitForSecondsRealtime(waitTime); //TODO calcular según animación
            sprite.enabled = false;
        }
    }
}
