using UnityEngine;
using UnityEngine.Events;

public class OnTrigger2D : MonoBehaviour
{

    [SerializeField] string targetTag = "Player";
    [SerializeField] UnityEvent OnEnter2D;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
        {
            OnEnter2D.Invoke();
        }
    }
}
