using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room2Spawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public SpawnData[] spawnDatas;
    float timer;
    private void Awake()
    {
        spawnPoints = GetComponentsInChildren<Transform>();
        timer = 0;
    }
}



[System.Serializable]
public class SpawnData
{
    public int prefabID;
    public float maxHealth;
    public float speed;
}