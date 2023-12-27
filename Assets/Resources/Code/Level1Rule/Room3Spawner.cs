using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room3Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;
    public float levelTime;
    public Room3Pool pool;


    int count;
    float timer;
    int level;
    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();

    }
    private void OnEnable()
    {
        timer = 0;
        count = 1;
    }
    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (timer > 1.2f * count ) 
        {
            count++;
            Spawn(Random.Range(0, pool.prefabs.Length)); 
        }

        


    }
    void Spawn(int index)
    {
        //lenh spawn enemy 
        GameObject enemy = pool.Get(index);
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
    }
}
