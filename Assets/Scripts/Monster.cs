using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public static Monster Instance;
    [SerializeField] private Transform DeathZonePosition, Pos1, Pos2, Pos3;
    [SerializeField] private float fallbackSpeed;
    [SerializeField] private Animator _animator;
    private int _dangerLevel = 1;
    private bool dangerLevelLowers = false;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
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
            StartCoroutine("LowerDangerLevel");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(DeathZonePosition.position, new Vector3(4, 13, 1));
    }

    public void MonsterCloseIn()
    {
        _dangerLevel++;
        if (_dangerLevel > 3)
        {
            UIManagerInGame.Instance.ChangeGameState(UIManagerInGame.GameState.Death);
            return;
        }

        switch (_dangerLevel)
        {
            case 1: break;
            case 2:
                transform.position = new Vector3(Pos2.position.x, transform.position.y, transform.position.z);
                break;
            case 3:
                transform.position = new Vector3(Pos3.position.x, transform.position.y, transform.position.z);
                break;
        }

        _animator.SetTrigger("Roar");
    }

    private IEnumerator LowerDangerLevel()
    {
        dangerLevelLowers = true;
        yield return new WaitForSeconds(5f * _dangerLevel);
        _dangerLevel--;
        dangerLevelLowers = false;
    }
}
