using UnityEngine;

public class TipsToggler : MonoBehaviour
{
    private bool _triggered = false;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player") && !_triggered)
        {
            UIManagerInGame.Instance.ChangeGameState(UIManagerInGame.GameState.Play);
            _triggered = true;
        }
    }
}
