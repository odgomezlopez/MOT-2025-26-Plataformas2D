using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    [SerializeField] float gameOverDelay = 0.5f;
    [SerializeField] UnityEvent OnGameOver;

    PlayerController controller;
    StatsComponent stats;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //Inicializamos las referencias
        controller = FindFirstObjectByType<PlayerController>();
        stats = controller?.GetComponent<StatsComponent>();
    }

    #region Suscripciones
    //Me suscribo/desuscribo de los cambio de HP
    private void OnEnable()
    {
        if (stats) stats.stats.HP.OnValueChanged += UpdateHP;
    }

    private void OnDisable()
    {
        if (stats) stats.stats.HP.OnValueChanged -= UpdateHP;
    }
    #endregion

    #region GameOver
    //Encargado de actualizar HP
    private void UpdateHP(float current, float max, float oldValue = 0)
    {
        if (current == 0) StartCoroutine(GameOverCoroutine());
    }

    public void Win() { 
        Debug.Log("Has ganado!");
    }

    public void GameOver() { 
        StartCoroutine(GameOverCoroutine()); 
    }

    private IEnumerator GameOverCoroutine()
    {
        OnGameOver?.Invoke();//Asociar efectos, filtros, etc.

        yield return new WaitForSecondsRealtime(gameOverDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion

}
