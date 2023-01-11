using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllusionMovement : MonoBehaviour
{
    [SerializeField] private float _levelSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3( _levelSpeed * -Time.deltaTime,0,0);
    }
}
