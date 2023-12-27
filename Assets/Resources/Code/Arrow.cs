using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float damage;

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

    public void Init(float damage, Vector3 dir)
    {
        this.damage = damage;
        rigid.velocity = dir * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            gameObject.SetActive(false);
        }

        if (!collision.CompareTag("Enemy")) return;

            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Ground")) return;
        gameObject.SetActive(false);
    }
}
