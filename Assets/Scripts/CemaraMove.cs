using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CemaraMove : MonoBehaviour
{
    [SerializeField] public Transform followTarget;
    
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(followTarget.position.x, transform.position.y, transform.position.z);
    }
}
