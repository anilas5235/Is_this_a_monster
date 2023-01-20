using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObstacleGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] JumpObstacles, SlideObstacles;
    private List<GameObject> spawnedObstacles = new List<GameObject>();
    private float nextTimeToSpawn = 0;

    private void Update()
    {
        if (Time.time > nextTimeToSpawn)
        {
            GenerateObstacle();
            nextTimeToSpawn += Random.Range(1f,4f);
            Debug.Log("obst");
        }

        if (spawnedObstacles == null) { return; }
        if (spawnedObstacles.Count < 1) { return; }
        if (spawnedObstacles[0].transform.position.x < -30)
        {
            GameObject del = spawnedObstacles[0];
            spawnedObstacles.RemoveAt(0);
            Destroy(del);
        }

        
    }

    private void GenerateObstacle()
    {
        bool ObstTyp = 0.4 < Random.Range(0f,1f);
        if (ObstTyp)
        {
            int index = Random.Range(0, JumpObstacles.Length);

            GameObject obst = Instantiate(JumpObstacles[index], new Vector3(30,0,0),quaternion.identity);
            Vector3 offset = new Vector3(30,0,0);offset.x += Random.Range(-2f, 2f); 
            switch (index)
            {
                case 0: offset.y = -7.83f;  break;
                case 1: offset.y = -7.76f; break;
                case 2: offset.y = -7.85f; break;
                case 3: offset.y = -8.24f; break;
            }
            obst.transform.position = offset;
            obst.transform.SetParent(gameObject.transform);
            spawnedObstacles.Add(obst);
        }
        else
        {
            int index = Random.Range(0, SlideObstacles.Length);

            GameObject obst = Instantiate(SlideObstacles[index], gameObject.transform);
            Vector3 offset = new Vector3(30,0,0);offset.x += Random.Range(-2f, 2f); 
            switch (index)
            {
                case 0: offset.y = -3.68f; break;
                case 1: offset.y = -3.68f; break;
                case 2: offset.y = -3.68f; break;
                case 3: offset.y = -0.76f; break;
            }
            obst.transform.position = offset;
            obst.transform.SetParent(gameObject.transform);
            spawnedObstacles.Add(obst);
        }
    }
}
