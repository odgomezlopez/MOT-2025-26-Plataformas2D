using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameData : MonoBehaviourSingleton<GameData>
{
    public ObservableValue<int> nMuertes;

    protected override void Awake()
    {
        base.Awake();
        nMuertes.Value = PlayerPrefs.GetInt("nMuertes", 0);
    }

    private void OnEnable()
    {
        if(GameManager.Instance) GameManager.Instance.OnGameOver.AddListener(IncrementarMuertes);
        //Suscribir a los cambios de escena para actualizar la suscripcion al evento de GameOver del GameManager
        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
    }
    private void OnDisable()
    {
        //Desuscribirse del evento de GameOver del GameManager y de los cambios de escena
        if (GameManager.Instance) GameManager.Instance.OnGameOver.RemoveListener(IncrementarMuertes);
        SceneManager.activeSceneChanged -= SceneManager_activeSceneChanged;
    }

    private void IncrementarMuertes()
    {
        nMuertes.Value++;
        PlayerPrefs.SetInt("nMuertes", nMuertes.Value);
    }

    //Si se cambia de escena, miro si hay algun GameManager para suscribirme a su evento de GameOver, y si no lo hay, me desuscribo de cualquier evento anterior
    private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
    {
        if (GameManager.Instance == null) return;
        try
        {
            GameManager.Instance.OnGameOver.RemoveListener(IncrementarMuertes);
        }
        catch { }
        finally
        {
            if(GameManager.Instance) GameManager.Instance.OnGameOver.AddListener(IncrementarMuertes);
        }
    }
}
