using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float damage;
    public int per;
    public float speed;
    float animCD;
    Rigidbody2D rigid;
    Vector3 dir;
    Transform transform;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        transform = GetComponent<Transform>();
    }
    private void OnEnable()
    {
        animCD = 0.85f;
    }

    public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage;
        this.per = per;
        rigid.velocity = dir * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            gameObject.SetActive(false);
        }
        if (!collision.CompareTag("Player") || per == -100) return; 
        else
        {
            collision.GetComponent<Player>().health -= damage;
            per--;

            if (per < 0)
            {
                rigid.velocity = Vector2.zero;
                gameObject.SetActive(false);
            }

        }

        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Ground")) return;
        gameObject.SetActive(false);
    }
}
