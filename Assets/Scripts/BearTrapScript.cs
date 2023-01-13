
using System.Collections;
using UnityEngine;

public class BearTrapScript : MonoBehaviour
{
    private bool triggerd = false;
    private GameObject Player;
    [SerializeField] private float holdDuration;
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
        Player.transform.position = new Vector3(transform.position.x, transform.position.y, Player.transform.position.z);
        Player.transform.SetParent(this.transform);
        yield return new WaitForSeconds(holdDuration);
        Player.transform.SetParent(null);
    }
}
