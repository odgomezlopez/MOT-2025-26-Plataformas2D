using System;
using UnityEngine;

public class WinWithScore : MonoBehaviour
{

    [SerializeField] int scoreToWin = 1;

    private void OnEnable()
    {
        if(ScoreManager.Instance) ScoreManager.Instance.Score.OnValueChanged += CheckWinCondition;
    }

    private void OnDisable()
    {
        if(ScoreManager.Instance) ScoreManager.Instance.Score.OnValueChanged -= CheckWinCondition;
    }

    private void CheckWinCondition(int currentScore)
    {
        if(currentScore >= scoreToWin)
        {
            GameManager.Instance.Win();
        }
    }
}
