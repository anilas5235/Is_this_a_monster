using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Random = UnityEngine.Random;

public class Monster : MonoBehaviour
{
    public static Monster Instance;
    [SerializeField] private Transform DeathZonePosition, Pos1, Pos2, Pos3;
    [SerializeField] private float fallbackSpeed;
    [SerializeField] private Animator _animator, _deathAnimator;
    [SerializeField] private VideoPlayer _roar, _trapDeath, _growl;

    private int _dangerLevel = 1;
    public int deathID =1;
    private bool dangerLevelLowers = false;
    private Coroutine DangerMeterLowering;
    private float waitTime = 10f;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Invoke( "Growl",waitTime);
        MonsterCloseIn();
    }

    void Update()
    {
        if (UIManagerInGame.Instance.currGameState != UIManagerInGame.GameState.TipsOn &&
            UIManagerInGame.Instance.currGameState != UIManagerInGame.GameState.Play) { return; }
        
        Collider2D col = Physics2D.OverlapBox(DeathZonePosition.position, new Vector2(4, 13), 0);
        if (col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                UIManagerInGame.Instance.ChangeGameState(UIManagerInGame.GameState.Death);
            }
        }

        if (transform.position.x > Pos1.position.x)
        {
            transform.position -= new Vector3(Time.deltaTime * fallbackSpeed * (1 / _dangerLevel), 0, 0);
        }

        if (_dangerLevel > 1 && !dangerLevelLowers)
        {
            DangerMeterLowering = StartCoroutine("LowerDangerLevel");
        }
    }

    public void MonsterCloseIn()
    {
        _dangerLevel++;
        if (_dangerLevel > 3)
        {
            UIManagerInGame.Instance.ChangeGameState(UIManagerInGame.GameState.Death);
            _deathAnimator.SetInteger("deathID",deathID);
            switch (deathID)
            {
                case 2: _trapDeath.Play(); break;
            }
            return;
        }

        if (DangerMeterLowering != null)
        {
            StopCoroutine(DangerMeterLowering);dangerLevelLowers = false;
            DangerMeterLowering = null;
        }

        switch (_dangerLevel)
        {
            case 1: break;
            case 2: transform.position = new Vector3(Pos2.position.x, transform.position.y, transform.position.z); break;
            case 3: transform.position = new Vector3(Pos3.position.x, transform.position.y, transform.position.z); break;
        }
        _animator.SetTrigger("Roar");
        _roar.Play();
    }

    private IEnumerator LowerDangerLevel()
    {
        dangerLevelLowers = true;
        yield return new WaitForSeconds(5f * _dangerLevel);
        _dangerLevel--;
        dangerLevelLowers = false;
        DangerMeterLowering = null;
    }

    private void Growl()
    {
        if (UIManagerInGame.Instance.currGameState != UIManagerInGame.GameState.Death &&
            UIManagerInGame.Instance.currGameState != UIManagerInGame.GameState.Win)
        {
            _growl.Play();
            waitTime = Random.Range(10, 30);
            Invoke( "Growl",waitTime);
        }
    }
}
