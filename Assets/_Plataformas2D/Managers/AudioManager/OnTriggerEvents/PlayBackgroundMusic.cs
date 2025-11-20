using UnityEngine;
using UnityEngine.Audio;

public class PlayBackgroundMusic : MonoBehaviour
{
    [SerializeField] string targetTag = "Player";
    [SerializeField] AudioResource newBackgroundMusic;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
        {
            AudioManager.I.PlayBackground(newBackgroundMusic);
            gameObject.SetActive(false);
        }
    }
}
