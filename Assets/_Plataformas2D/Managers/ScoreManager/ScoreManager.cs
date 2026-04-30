public class ScoreManager : MonoBehaviourSingleton<ScoreManager>
{
    public ObservableValue<int> Score;
    private int lastScore = 0;

    public void AddScore(int amount)
    {
        Score.Value += amount;
    }

    public void SaveScore()
    {
        lastScore = Score.Value;
    }

    public void ResetScore()
    {
        Score.Value = lastScore;
    }

    private void OnValidate()
    {
        Score.Notify();
    }



}
