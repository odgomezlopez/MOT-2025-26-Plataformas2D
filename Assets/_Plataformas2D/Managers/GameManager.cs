using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    //Win
    [Scene, SerializeField] string nextScene;
    [SerializeField, Range(0f,3f)] float winDelay = 0.5f;
    [SerializeField] UnityEvent OnWin;

    //GameOver
    [SerializeField, Range(0f, 3f)] float gameOverDelay = 0.5f;
    [SerializeField] public UnityEvent OnGameOver;

    //Referencias
    PlayerController controller;
    PlayerStats stats;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //Inicializamos las referencias
        controller = FindFirstObjectByType<PlayerController>();
        stats = (PlayerStats) GameData.Instance.GetComponent<IStatsComponent>().Stats;

        //if (stats.HP.Value == 0) 
            stats.HP.Reset();
    }

    #region Suscripciones
    //Me suscribo/desuscribo de los cambio de HP
    private void OnEnable()
    {
        if (stats != null) stats.HP.OnValueChanged += UpdateHP;
    }

    private void OnDisable()
    {
        if (stats != null) stats.HP.OnValueChanged -= UpdateHP;
    }
    #endregion

    #region GameOver
    //Encargado de actualizar HP
    private void UpdateHP(float current, float max, float oldValue = 0)
    {
        if (current == 0) StartCoroutine(GameOverCoroutine());
    }

    public void Win() {
        StartCoroutine(WinCoroutine());
    }

    public void GameOver() { 
        StartCoroutine(GameOverCoroutine()); 
    }
    private IEnumerator WinCoroutine()
    {
        OnWin?.Invoke();//Asociar efectos, filtros, etc.
        ScoreManager.Instance.SaveScore();
        InventoryManager.Instance.SaveInventory();

        yield return new WaitForSecondsRealtime(winDelay);
        SceneLoaderManager.Instance.LoadScene(nextScene);
    }

    private IEnumerator GameOverCoroutine()
    {
        OnGameOver?.Invoke();//Asociar efectos, filtros, etc.
        ScoreManager.Instance.ResetScore();
        InventoryManager.Instance.ResetInventory();


        yield return new WaitForSecondsRealtime(gameOverDelay);
        SceneLoaderManager.Instance.RestartScene();
        //ScoreManager.Instance.Score.Value = 0;
    }
    #endregion

}
