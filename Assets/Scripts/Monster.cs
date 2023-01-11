using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] private Transform DeathZonePosition;
    void Update()
    {
        Collider2D col = Physics2D.OverlapBox(DeathZonePosition.position, new Vector2(4, 13), 0);
        Debug.Log(""+col);
        if (col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                Time.timeScale = 0;
                Debug.Log("You died");
                UIManagerInGame.Instance.MenuChangeState(UIManagerInGame.GameState.Death);
                //death screen trigger
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(DeathZonePosition.position,new Vector3(4,13,1));
    }
}
