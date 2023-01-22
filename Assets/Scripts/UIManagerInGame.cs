using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIManagerInGame : MonoBehaviour
{
    public static UIManagerInGame Instance;

    public float distanceRun;

    [SerializeField] private GameObject pauseScreenController, tipsController, deathScreenController, winScreenController;

    [SerializeField, ReadOnly] private float _currentTimeScale;

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
        _currentTimeScale = 1f;
        Time.timeScale = _currentTimeScale;
        ChangeGameState(GameState.TipsOn);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel") && currGameState != GameState.Pause)
        {
            ChangeGameState(GameState.Pause);
        }
    }

    private void FixedUpdate()
    {
        if (currGameState == GameState.Play || currGameState == GameState.TipsOn)
        { 
            _currentTimeScale += 0.01f * Time.deltaTime;
            Time.timeScale = _currentTimeScale;
        }
    }

    public void ChangeGameState(GameState newState)
    {
        switch (currGameState)
        {
            case GameState.Play: 
            case GameState.Pause: pauseScreenController.SetActive(false); Time.timeScale = _currentTimeScale;  break;
            case GameState.Death: deathScreenController.SetActive(false);  break;
            case GameState.Win: winScreenController.SetActive(false);  break;
            case GameState.TipsOn: tipsController.SetActive(false); break;
            default: print(" Error, Menu does not exist");break;
            
        }
        currGameState = newState;
        
        switch (currGameState)
        {
            case GameState.Play: Time.timeScale = 1;  break;
            case GameState.Pause:Time.timeScale = 0; pauseScreenController.SetActive(true);  break;
            case GameState.Death:Time.timeScale = 1; deathScreenController.SetActive(true); break;
            case GameState.Win: Time.timeScale = 1; winScreenController.SetActive(true);  break;
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
}
