using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{

    private bool _triggered = false;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player") && !_triggered)
        {
            UIManagerInGame.Instance.ChangeGameState(UIManagerInGame.GameState.Win);
            _triggered = true;
        }
    }
}


