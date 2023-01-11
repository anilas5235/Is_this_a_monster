using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIManagerInGame : MonoBehaviour
{
    public static UIManagerInGame Instance;

    [SerializeField] private GameObject pauseScreenController, tipsController, deathScreenController, winScreenController;

    public enum GameState
    {
        Play =0,
        Pause =1,
        Death =2,
        Win =3,
    }

    public GameState currGameState;

    private void Awake()
    {
        Instance = this;
    }

    public void MenuChangeState(GameState newState)
    {
        switch (currGameState)
        {
            case GameState.Play: tipsController.SetActive(false); break;
            case GameState.Pause: pauseScreenController.SetActive(false); break;
            case GameState.Death: deathScreenController.SetActive(false); break;
            case GameState.Win: winScreenController.SetActive(false); break;
            default: print(" Error, Menu does not exist");break;
        }
        currGameState = newState;
        
        switch (currGameState)
        {
            case GameState.Play: tipsController.SetActive(true); break;
            case GameState.Pause: pauseScreenController.SetActive(true); break;
            case GameState.Death: deathScreenController.SetActive(true); break;
            case GameState.Win: winScreenController.SetActive(true); break;
        }
    }

    public void ButtonToMenuTranslate(int menuID)
    {
        GameState newState;
        switch (menuID)
        {
            case 0: newState = GameState.Play; break;
            case 1: newState = GameState.Pause; break;
            case 2: newState = GameState.Death; break;
            case 3: newState = GameState.Win; break;
            default: Debug.Log("menuID does not exist"); return;
        }
        MenuChangeState(newState);
    }

    public void LoadLevel(int level)
    {
        string sceneName;
        switch (level)
        {
            case 3: sceneName = "Level3";break;
            default: Debug.Log("failed to load Scene, scene is not recognized"); return;
        }
        SceneManager.LoadScene(sceneName);
    }
}
