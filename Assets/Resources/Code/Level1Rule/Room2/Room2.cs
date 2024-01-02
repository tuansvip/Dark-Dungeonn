using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room2 : MonoBehaviour
{
    public bool isClear;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isClear || !collision.CompareTag("Player")) return;
        
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
    }
    private void FixedUpdate()
    {
        if (isClear)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
        if (transform.GetChild(1).GetComponent<CheckClear>().isClear)
        {
            transform.GetChild(2).gameObject.SetActive(true);
        }
        if (transform.GetChild(1).GetComponent<CheckClear>().isClear && transform.GetChild(2).GetComponent<CheckClear>().isClear)
        {
            isClear = true;
            SFXPlay.instance.PlayWin();
        }
    }
}
