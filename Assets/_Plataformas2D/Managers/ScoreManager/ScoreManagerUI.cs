using TMPro;
using UnityEngine;

public class ScoreManagerUI : MonoBehaviour
{
    [SerializeField]TextMeshProUGUI scoreText;

    private void Awake()
    {
        if (scoreText == null) scoreText = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable() { 
        ScoreManager.Instance.Score.OnValueChanged += UpdateScoreText;
        ScoreManager.Instance.Score.Notify();
    }

    private void OnDisable()
    {
        ScoreManager.Instance.Score.OnValueChanged -= UpdateScoreText;
    }

    private void UpdateScoreText(int newScore)
    {
        scoreText.text = newScore.ToString("000");
    }
}
