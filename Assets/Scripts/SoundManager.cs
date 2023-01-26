using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioSource ChaseStart, ChaseLoop, ChaseClose;
    [SerializeField] private VideoPlayer _Intro, _normalDeath, _trapDeath, _win, _fastHeard;
    private int _currentlyPlayingMusicIndex, _currentPlayingCutSceneSound = -1;
    private Coroutine _currentSoundChange;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
    }

    private void ChangeMusic(int index)
    {
        switch (_currentlyPlayingMusicIndex)
        {
            case-1: break;
            case 0: ChaseStart.Stop(); break; 
            case 1: ChaseLoop.Stop(); break; 
            case 2: ChaseClose.Stop(); _fastHeard.Stop(); break; 
        }
        _currentlyPlayingMusicIndex = index;
        
        switch (_currentlyPlayingMusicIndex)
        {
            case-1: break;
            case 0: ChaseStart.Play(); MusicChangePublicAccess(1,6f); break; 
            case 1: ChaseLoop.Play(); break; 
            case 2: ChaseClose.Play(); _fastHeard.Play(); MusicChangePublicAccess(1, 15f); break; 
        }
    }

    private IEnumerator ChangeMusicAfterTime(int index, float time)
    {
        yield return new WaitForSecondsRealtime(time);
        _currentSoundChange = null;
        ChangeMusic(index);
    }

    public void MusicChangePublicAccess(int id, float time)
    {
        if(_currentSoundChange != null) {StopCoroutine(_currentSoundChange); _currentSoundChange = null; } 
        _currentSoundChange = StartCoroutine(ChangeMusicAfterTime(id, time));
    }

    public void TriggerCutSceneAudio(int index)
    {
        switch (_currentPlayingCutSceneSound)
        {
            case 0:  _Intro.Stop(); break; //IntroSound
            case 1:   break; //deathNormal
            case 2: _trapDeath.Stop(); break; //deathBearTrap
            case 3: _win.Stop();  break; //winCutScene
            case 4: _fastHeard.Stop(); break;
        }
        _currentPlayingCutSceneSound = index;
        if( _currentPlayingCutSceneSound != 4) {MusicChangePublicAccess( -1, 0f);}
        switch (_currentPlayingCutSceneSound)
        {
            case 0:  _Intro.Play(); break; //IntroSound
            case 1:   break; //deathNormal
            case 2: _trapDeath.Play(); break; //deathBearTrap
            case 3: _win.Play();  break; //winCutScene
            case 4: _fastHeard.Play(); break;
        }
    }
}
