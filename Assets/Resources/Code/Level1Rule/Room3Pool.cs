using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room3Pool : MonoBehaviour
{
    public GameObject[] prefabs;
    public Transform target;
    List<GameObject>[] pools;

    private void Awake()
    {

        pools = new List<GameObject>[prefabs.Length];
        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        if (!select)
        {
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }
        Enemy enem = select.GetComponent<Enemy>();
        enem.target = target;
        return select;
    }
}
