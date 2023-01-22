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

    private void Start()
    {
        nextTimeToSpawn = Time.time + Random.Range(1.5f,3.5f);
    }

    private void Update()
    {
        if (UIManagerInGame.Instance.currGameState != UIManagerInGame.GameState.TipsOn &&
            UIManagerInGame.Instance.currGameState != UIManagerInGame.GameState.Play) { return; }
        
        if (Time.time > nextTimeToSpawn)
        {
            GenerateObstacle();
            nextTimeToSpawn += Random.Range(1.5f,3.5f);
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
                case 0: offset.y = -6.78f+ Random.Range(-0.3f,0.3f); break; //log
                case 1: offset.y = -6.81f+ Random.Range(-0.3f,0.3f); break; //root
                case 2: offset.y = -6.9f + Random.Range(-0.3f,0.3f);  break; //stone
                case 3: offset.y = -7.29f; break; //bear trap
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
                case 0: offset.y = -2.73f + Random.Range(-0.3f,0.3f); break; //branch
                case 1: offset.y = -2.73f + Random.Range(-0.3f,0.3f); break; //branch
                case 2: offset.y = -2.73f + Random.Range(-0.3f,0.3f); break; //branch
                case 3: offset.y =  0.21f + Random.Range(-0.3f,0.3f); break; //liana
                case 4: offset.y =  -3.6f + Random.Range(-0.3f,0.3f); break; //fallen tree
            }
            obst.transform.position = offset;
            obst.transform.SetParent(gameObject.transform);
            spawnedObstacles.Add(obst);
        }
    }
}
