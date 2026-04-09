public class ScoreManager : MonoBehaviourSingleton<ScoreManager>
{
    public ObservableValue<int> Score;
    
    public void AddScore(int amount)
    {
        Score.Value += amount;
    }

    private void OnValidate()
    {
        Score.Notify();
    }
}
