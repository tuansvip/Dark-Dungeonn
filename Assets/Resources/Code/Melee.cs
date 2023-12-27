using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Melee : MonoBehaviour
{
    public float damage;
    public float maxTime;
    float timeCD;
    Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    public void Init(float damage, Vector3 dir)
    {
        this.damage = damage;
        rigid.velocity = dir * 7f;
        timeCD = maxTime;
    }
    private void FixedUpdate()
    {
        timeCD -= Time.fixedDeltaTime;
        if (timeCD < 0 )
        {
            gameObject.SetActive(false);
        }
       
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Ground")) return;
        gameObject.SetActive(false);
    }

}
