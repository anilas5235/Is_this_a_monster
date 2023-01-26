using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepsPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip[] stepsSounds;
    [SerializeField] private AudioSource stepsSource;

    public void TriggerFootStep()
    {
        int a = Random.Range(0, stepsSounds.Length);
        stepsSource.PlayOneShot(stepsSounds[a]);
    }
}
