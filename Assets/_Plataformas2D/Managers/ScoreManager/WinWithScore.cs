using System;
using UnityEngine;

public class WinWithScore : MonoBehaviour
{

    [SerializeField] int scoreToWin = 1;

    private void OnEnable()
    {
        ScoreManager.Instance.Score.OnValueChanged += CheckWinCondition;
    }

    private void OnDisable()
    {
        ScoreManager.Instance.Score.OnValueChanged -= CheckWinCondition;
    }

    private void CheckWinCondition(int currentScore)
    {
        if(currentScore >= scoreToWin)
        {
            GameManager.Instance.Win();
        }
    }
}
