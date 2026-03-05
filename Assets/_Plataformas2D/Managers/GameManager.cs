using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
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
        if (current == 0) StartCoroutine(GameOver());
    }

    private IEnumerator GameOver()
    {
        OnGameOver?.Invoke();//Asociar efectos, filtros, etc.
        yield return new WaitForSecondsRealtime(gameOverDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion

}
