using UnityEngine;

public class FootstepsPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip[] stepsSounds;
    [SerializeField] private AudioSource stepsSource;

    public void TriggerFootStep()
    {
        if(UIManagerInGame.Instance.currGameState != UIManagerInGame.GameState.Play){return;}
        int a = Random.Range(0, stepsSounds.Length);
        stepsSource.PlayOneShot(stepsSounds[a]);
    }
}
