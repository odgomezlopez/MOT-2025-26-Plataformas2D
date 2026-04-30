using UnityEngine;

public class OnlyShowOnce : MonoBehaviour
{
    string key;
    private void Awake()
    {
        key = gameObject.GetInstanceID().ToString();
        if (PlayerPrefs.GetInt(key, 0) == 1)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerPrefs.SetInt(key, 1);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            PlayerPrefs.SetInt(key, 1);
            Destroy(gameObject);
        }
    }

}
