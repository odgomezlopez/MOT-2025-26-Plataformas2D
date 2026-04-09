using UnityEngine;

public class AddScoreOnEnter : MonoBehaviour
{
    [SerializeField] int scoreAmount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ScoreManager.Instance.AddScore(scoreAmount);
            //ScoreManager.Instance.Score.Value += scoreAmount;

            Destroy(this);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            ScoreManager.Instance.AddScore(scoreAmount);
            //ScoreManager.Instance.Score.Value += scoreAmount;
            Destroy(this);
        }
    }

    private void OnValidate()
    {
        //if (scoreAmount < 0) scoreAmount = 0;
    }
}
