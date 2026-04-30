using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderManager : MonoBehaviourSingleton<SceneLoaderManager>
{
    List<string> excludeScenes = new List<string>() { "MainMenu", "GameOver" };

    public void LoadScene(int buildIndex)
    {
        string sceneName = SceneManager.GetSceneByBuildIndex(buildIndex).name;
        LoadScene(sceneName);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);

        //Guardar Ultima escena para cargarla despues del GameOver o del Win, pero si la escena a cargar es el MainMenu o el GameOver, no la guarda
        if (excludeScenes.Contains(sceneName)) return;
        PlayerPrefs.SetString("LastScene", sceneName);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
