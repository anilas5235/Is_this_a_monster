using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManagerTileScreen : MonoBehaviour
{

    [SerializeField] private AudioMixer mainAudioMixer;
    [SerializeField] private Slider main, music, monster, effects;
    [SerializeField] private GameObject startScreenController, optionsSelectController, audioOptionsController, levelSelectController;
    [SerializeField] private TitleScreenButton[] _levelButtons;

    public enum Menu
    {
        OptionsSelect =0,
        StartScreen =1,
        AudioOptions =2,
        LevelSelect =3,
    }

    public Menu CurrMenu;

    private void Start()
    {
        MenuChangeState(Menu.StartScreen);
        LoadFromSaveText();
        UpdateSoundOptions();
    }


    // Update is called once per frame
    private  void Update()
    {
        switch (CurrMenu)
        {
            case Menu.OptionsSelect: break;
            case Menu.StartScreen: break;
            case Menu.AudioOptions: UpdateSoundOptions(); break;
            case Menu.LevelSelect: break;
            default: print(" Error, Menu does not exist");break;
        }
    }

    public void MenuChangeState(Menu newState)
    {
        switch (CurrMenu)
        {
            case Menu.OptionsSelect: optionsSelectController.SetActive(false); break;
            case Menu.StartScreen: startScreenController.SetActive(false); Time.timeScale = 1; break;
            case Menu.AudioOptions: SaveOptionsToText(); audioOptionsController.SetActive(false); SaveSystem.instance.Save(); break;
            case Menu.LevelSelect: levelSelectController.SetActive(false); break;
            default: print(" Error, Menu does not exist");break;
        }
        CurrMenu = newState;
        
        switch (CurrMenu)
        {
            case Menu.OptionsSelect: optionsSelectController.SetActive(true); break;
            case Menu.StartScreen: startScreenController.SetActive(true); break;
            case Menu.AudioOptions: audioOptionsController.SetActive(true); break;
            case Menu.LevelSelect: levelSelectController.SetActive(true); OpenLevelSelect(); break;
        }
    }

    public void ButtonToMenuTranslate(int menuID)
    {
        Menu newState;
        switch (menuID)
        {
            case 0: newState = Menu.OptionsSelect; break;
            case 1: newState = Menu.StartScreen; break;
            case 2: newState = Menu.AudioOptions; break;
            case 3: newState = Menu.LevelSelect; break;
            default: Debug.Log("menuID does not exist"); return;
        }
        MenuChangeState(newState);
    }

    private void UpdateSoundOptions()
    {
        mainAudioMixer.SetFloat("Master", main.value);
        mainAudioMixer.SetFloat("Monster", monster.value);
        mainAudioMixer.SetFloat("Music", music.value);
        mainAudioMixer.SetFloat("Effects", effects.value);
    }

    private void SaveOptionsToText()
    {
        mainAudioMixer.GetFloat("Master", out SaveSystem.instance.GetActiveSave().audioOptions[0]);
        mainAudioMixer.GetFloat("Music", out SaveSystem.instance.GetActiveSave().audioOptions[1]);
        mainAudioMixer.GetFloat("Monster", out SaveSystem.instance.GetActiveSave().audioOptions[2]);
        mainAudioMixer.GetFloat("Effects", out SaveSystem.instance.GetActiveSave().audioOptions[3]);
    }
    private void LoadFromSaveText()
    {
        float[] optionsValues = SaveSystem.instance.GetActiveSave().audioOptions;
        mainAudioMixer.SetFloat("Master", optionsValues[0]);
        mainAudioMixer.SetFloat("Music", optionsValues[1]);
        mainAudioMixer.SetFloat("Monster", optionsValues[2]);
        mainAudioMixer.SetFloat("Effects", optionsValues[3]);

        main.value = optionsValues[0];
        music.value = optionsValues[1];
        monster.value = optionsValues[2];
        effects.value = optionsValues[3];
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
        SceneManager.LoadScene(sceneName);
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    private void OpenLevelSelect()
    {
        bool[] unlocked = SaveSystem.instance.GetActiveSave().levelsUnlocked;
        for (int i = 0; i < _levelButtons.Length; i++)
        {
            _levelButtons[i].isUnlocked = unlocked[i];
        }
    }
}
