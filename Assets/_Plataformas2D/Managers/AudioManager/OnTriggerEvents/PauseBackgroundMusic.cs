using UnityEngine;
using UnityEngine.Events;

public class PauseBackgroundMusic : MonoBehaviour
{
    [SerializeField] string targetTag = "Player";
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
        {
            AudioManager.I.PauseBackround();
            gameObject.SetActive(false);
        }
    }
}
