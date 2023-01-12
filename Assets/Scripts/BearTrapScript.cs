
using System.Collections;
using UnityEngine;

public class BearTrapScript : MonoBehaviour
{
    private bool triggerd = false;
    private GameObject Player;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player")&& !triggerd)
        {
            Debug.Log("Bear Trap Triggered");
           Player =col.gameObject;
          StartCoroutine( TriggerBearTrap());
        }
    }

    private IEnumerator TriggerBearTrap()
    {
        Player.transform.SetParent(this.transform);
        yield return new WaitForSeconds(4f);
        Player.transform.SetParent(null);
    }
}
