using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIManagerInGame : MonoBehaviour
{
    public static UIManagerInGame Instance;

    public float distanceRun;

    [SerializeField] private GameObject pauseScreenController, tipsController, deathScreenController, winScreenController, introController, monster;
    [SerializeField] private ObstacleGenerator obstacleGenerator;
    [SerializeField] private IllusionMovement[] _runningLayers;
    [SerializeField, ReadOnly] private float _currentTimeScale;
    [SerializeField] private int unlockedByThisLevel, highScoreSaveIndex;
    public bool level_Infinite = true; //true for Level mode; false for infinite mode
    public int levelLength;
    private float timeForDifficulty;
    private TextMeshProUGUI _score;

    public enum GameState
    {
        Play =0,
        Pause =1,
        Death =2,
        Win =3,
        TipsOn =4,
        Intro =5,
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
        
        if (levelLength < 1) { level_Infinite = false; }

        if (level_Infinite)
        {
            SetCycles();
            ChangeGameState(GameState.TipsOn);
        }
        else
        {
            ChangeGameState(GameState.Intro);
            _score = GameObject.Find("Score").GetComponent<TextMeshProUGUI>();
        }
    }

    private void Update()
    {
        
        if (Input.GetButtonDown("Cancel") && (currGameState == GameState.Play))
        {
            ChangeGameState(GameState.Pause);
        }
        
        if(currGameState != GameState.Play){return;}
        if (_score )
        {
            _score.text = "" + Mathf.Round( distanceRun) + " m";
        }
    }

    private void FixedUpdate()
    {
        if (currGameState == GameState.Play)
        {
            timeForDifficulty += Time.fixedDeltaTime;
            _currentTimeScale = 0.35f* Mathf.Log(timeForDifficulty,5f)+1.4f;
            Time.timeScale = _currentTimeScale;
        }

        if (distanceRun / 35.5f > levelLength*5 +4 && level_Infinite)
        {
            obstacleGenerator.enabled = false;
        }
    }

    public void ChangeGameState(GameState newState)
    {
        switch (currGameState)
        {
            case GameState.Play: break;
            case GameState.Pause: pauseScreenController.SetActive(false); Time.timeScale = _currentTimeScale;  break;
            case GameState.Death: deathScreenController.SetActive(false);  break;
            case GameState.Win: winScreenController.SetActive(false);  break;
            case GameState.TipsOn: tipsController.SetActive(false); break;
            case GameState.Intro: introController.SetActive(false); monster.SetActive(true); obstacleGenerator.enabled = true; break;
            default: print(" Error, Menu does not exist");break;
        }
        currGameState = newState;
        
        switch (currGameState)
        {
            case GameState.Play: Time.timeScale = 1; break;
            case GameState.Pause:Time.timeScale = 0; pauseScreenController.SetActive(true);  break;
            case GameState.Death:Time.timeScale = 1; deathScreenController.SetActive(true); StartCoroutine( CheckAndSaveForHighScore()); break;
            case GameState.Win: Time.timeScale = 1; winScreenController.SetActive(true); UnlockNextLevel(unlockedByThisLevel); SoundManager.Instance.TriggerCutSceneAudio(3); break;
            case GameState.TipsOn: tipsController.SetActive(transform); break;
            case GameState.Intro: introController.SetActive(true); monster.SetActive(false); obstacleGenerator.enabled = false;
               StartCoroutine(ChangeToGameStateAfterTime(23f, GameState.Play)); SoundManager.Instance.TriggerCutSceneAudio(0);
               SoundManager.Instance.MusicChangePublicAccess(0, 23f);
                break;
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
            case 5: newState = GameState.Intro; break;
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
            case 4: sceneName = "Level3+";break;
            default: Debug.Log("failed to load Scene, scene is not recognized"); return;
        }
        Debug.Log("Change Scene to "+sceneName);
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator ChangeToGameStateAfterTime(float waitTime, GameState newGameState)
    {
        yield return new WaitForSeconds(waitTime);
        ChangeGameState(newGameState);
    }

    private void SetCycles()
    {
        _runningLayers[0].amountOFCycles = levelLength*5 +4;
        _runningLayers[1].amountOFCycles = levelLength *2 +1;
        _runningLayers[2].amountOFCycles = levelLength -1;
        _runningLayers[3].amountOFCycles = levelLength * 7 + 6;
    }
    private void UnlockNextLevel(int levelIndex)
    {
        if(!level_Infinite){return;}
        SaveSystem.instance.GetActiveSave().levelsUnlocked[levelIndex] = true;
        SaveSystem.instance.Save();
    }

    private IEnumerator CheckAndSaveForHighScore()
    {
        if (!level_Infinite)
        {
            _score.enabled = false;
            float currentHighScore = SaveSystem.instance.GetActiveSave().highScoresForEndsLevels[highScoreSaveIndex];
            if (currentHighScore < distanceRun)
            {
                SaveSystem.instance.GetActiveSave().highScoresForEndsLevels[highScoreSaveIndex] = distanceRun;
            }

            yield return new WaitForSeconds(3f);
            _score.enabled = true;
            _score.text = "HighScore: " +
                          SaveSystem.instance.GetActiveSave().highScoresForEndsLevels[highScoreSaveIndex].ToString("0") + " m" +
                          Environment.NewLine + "Score: " + distanceRun.ToString("0") + " m";
            SaveSystem.instance.Save();
        }
    }
}
