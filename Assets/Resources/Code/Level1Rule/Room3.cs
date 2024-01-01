using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room3 : MonoBehaviour
{
    public static Room3 instance;
    public bool isClear;
    public Player player;
    public float timer;
    public float infotimer;
    public bool isRunning;
    public Room3Pool pool;
    public GameObject hud;
    public GameObject info;

    private void Awake()
    {
        isRunning = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isClear || !collision.CompareTag("Player")) return;
        infotimer = 0f;
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(true);
        hud.SetActive(true);
        isRunning = true;
    }
    private void FixedUpdate()
    {
        if (!GameManager.instance.isPause)
        {
            timer += Time.fixedDeltaTime;
        }
        if (infotimer >= 3f && isRunning)
        {
            info.SetActive(false);  
        }
        if (timer <= 0 && player.isLive == true)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(false);
            isRunning = false;
            isClear = true;
            hud.SetActive(false);
        }
    }
}
