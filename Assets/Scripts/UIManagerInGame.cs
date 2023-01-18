using System;
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
        TipsOn =4,
    }

    public GameState currGameState;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ToggleTips();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel") && currGameState != GameState.Pause)
        {
            ChangeGameState(GameState.Pause);
        }
    }

    public void ChangeGameState(GameState newState)
    {
        switch (currGameState)
        {
            case GameState.Play: 
            case GameState.Pause: pauseScreenController.SetActive(false); Time.timeScale = 1;  break;
            case GameState.Death: deathScreenController.SetActive(false);  break;
            case GameState.Win: winScreenController.SetActive(false);  break;
            case GameState.TipsOn: tipsController.SetActive(false); break;
            default: print(" Error, Menu does not exist");break;
            
        }
        currGameState = newState;
        
        switch (currGameState)
        {
            case GameState.Play: Time.timeScale = 1;  break;
            case GameState.Pause: pauseScreenController.SetActive(true); Time.timeScale = 0; break;
            case GameState.Death: deathScreenController.SetActive(true); break;
            case GameState.Win: winScreenController.SetActive(true); break;
            case GameState.TipsOn: tipsController.SetActive(transform); break;
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
            case 4: newState = GameState.TipsOn; break;
            default: Debug.Log("menuID does not exist"); return;
        }
        ChangeGameState(newState);
    }

    public void LoadLevel(int level)
    {
        string sceneName;
        switch (level)
        {
            case 0: sceneName = "Menu"; break;
            case 3: sceneName = "Level3";break;
            default: Debug.Log("failed to load Scene, scene is not recognized"); return;
        }
        Debug.Log("Change Scene to "+sceneName);
        SceneManager.LoadScene(sceneName);
    }

    public void ToggleTips()
    {
        ChangeGameState(currGameState == GameState.TipsOn ? GameState.Play : GameState.TipsOn);
    }
}
