using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Scenes")]
    [Scene] public string NewGameScene;
    [Scene] public string MainMenuScene;

    [Header("Main menu")]
    public GameObject MainPanel;
    public Selectable MainFirstSelected;

    [Header("Settings menu")]
    public GameObject SettingPanel;
    public Selectable SettingFirstSelected;

    [Header("Creditos menu")]
    public GameObject CreditsPanel;
    public Selectable CreditsFirstSelected;

    [Header("Controles menu")]
    public GameObject ControlsPanel;
    public Selectable ControlsFirstSelected;


    private void Start()
    {
        OpenMainMenu();
    }

    #region Funciones Botones
    public void StartNewGame()
    {
        SceneLoaderManager.Instance.LoadScene(NewGameScene);
    }

    public void LoadMainMenu()
    {
        //Guardamos el estado del juego
        //SaveLoadManager.Instance.SaveGame();

        if (PersistenSystem.Instance) DestroyImmediate(PersistenSystem.Instance.gameObject);

        SceneLoaderManager.Instance.LoadScene(MainMenuScene);
    }

    public void Quit()
    {
        #if UNITY_STANDALONE
        Application.Quit();
        #endif

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
    #endregion

    #region Manejo de paneles
    public void OpenSettings()
   {
        //Desactivamos menu principal
        if(MainPanel) MainPanel?.SetActive(false);

        //Activamos Settings
        if(SettingPanel) SettingPanel?.SetActive(true);
        if(SettingFirstSelected) SettingFirstSelected?.Select();
    }

    public void OpenCredits()
    {
        //Desactivamos menu principal
        if(MainPanel) MainPanel?.SetActive(false);

        //Activamos Credits
        if(CreditsPanel) CreditsPanel?.SetActive(true);
        if(CreditsFirstSelected) CreditsFirstSelected?.Select();
    }

    public void OpenControls()
    {
        //Desactivamos menu principal
        if(MainPanel) MainPanel?.SetActive(false);

        //Activamos Controls
        if(ControlsPanel) ControlsPanel?.SetActive(true);
        if(ControlsFirstSelected) ControlsFirstSelected?.Select();
    }

    public void OpenMainMenu()
    {
        //Desactivamos todos paneles
        if(SettingPanel) SettingPanel?.SetActive(false);
        if(CreditsPanel) CreditsPanel?.SetActive(false);
        if(ControlsPanel) ControlsPanel?.SetActive(false);

        //Reactivamos el panel del menu principal y seleccionamos el primer boton
        if(MainPanel) MainPanel?.SetActive(true);
        if(MainFirstSelected) MainFirstSelected?.Select();
    }
    #endregion
}
