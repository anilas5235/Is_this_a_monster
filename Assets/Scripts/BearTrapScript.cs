using UnityEngine;

public class BearTrapScript : MonoBehaviour
{
    private bool triggerd = false;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player")&& !triggerd)
        {
            triggerd = true;
            Monster.Instance.deathID = 2;
            Monster.Instance.MonsterCloseIn();
            Monster.Instance.MonsterCloseIn();
            Monster.Instance.MonsterCloseIn();
        }
    }

}
