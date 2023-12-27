using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Scripting.APIUpdating;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [Header("# Stats")]
    public float health;
    public float maxHealth;
    public float damage;
    public float speed;
    public bool isAlive;


    [Header("# Object")]
    public Transform target;
    public GameObject healthPotion;
    public GameObject powerPotion;

    float attackCoundown;
    WaitForFixedUpdate waitFix;
    Animator anim;
    RectTransform rectTransform;
    Rigidbody2D rigid;
    Vector2 dirVec;
    Collider2D coll;
    ParticleSystem bloodParticleSystem;
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rectTransform = GetComponent<RectTransform>();
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        bloodParticleSystem = GetComponent<ParticleSystem>();
    }
    private void OnEnable()
    {
        health = maxHealth;
        isAlive = true;
        gameObject.layer = 6;
    }
    private void FixedUpdate()
    {
        Move(isAlive);
        attackCoundown -= Time.fixedDeltaTime;
    }

    private void Move(bool isAlive)
    {
        if (!isAlive) return;
        
            dirVec = target.position - rectTransform.position;
            Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
            rigid.MovePosition(rigid.position + nextVec);
            rigid.velocity = Vector2.zero;
        
    }

    private void LateUpdate()
    {
        if (dirVec != Vector2.zero)
            anim.SetFloat("RunState", 0.25f);
        else anim.SetFloat("RunState", 0f);
        if (dirVec.x != 0)
        {
            if (dirVec.x < 0)
            {
                transform.localScale = new Vector3(1.3f, 1.3f, 1);
            }
            else
            {
                transform.localScale = new Vector3(-1.3f, 1.3f, 1);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((!collision.CompareTag("Arrow") && !collision.CompareTag("Melee")) || !isAlive) return;
        if (collision.CompareTag("Arrow"))
        {
            collision.gameObject.SetActive(false);
        }
        SFXPlay.instance.PlayHit();
        bloodParticleSystem.Play();
        Invoke("StopBloodSplatter", 0.05f);

        if (collision.CompareTag("Arrow"))
        health -= collision.GetComponent<Arrow>().damage;
        if (collision.CompareTag("Melee"))
        health -= collision.GetComponent<Melee>().damage;
        if (health <= 0f)
        {
            GetComponentInChildren<SortingGroup>().sortingOrder = 2;
            gameObject.layer = 0;
            gameObject.tag = "Untagged";
            anim.SetTrigger("Die");
            isAlive = false;
            coll.enabled = false;
            rigid.simulated = false;
            rigid.constraints = RigidbodyConstraints2D.FreezeAll;
            bool hDrop = false;
            bool pDrop = false;
            if (Random.Range(1, 100) <= 30) hDrop = true;
            if (Random.Range(1, 100) <= 20) pDrop = true;
            if (hDrop && pDrop)
            {
                if (Random.Range(0, 1) == 0) hDrop = false;
                else pDrop = false;
            }
            if (hDrop)
            {
                GameObject drop = Instantiate(healthPotion);
                drop.transform.position = transform.position;
            }            
            
            if (pDrop)
            {
                GameObject drop = Instantiate(powerPotion);
                drop.transform.position = transform.position;
            }

        }
        else
        {
            StartCoroutine(KnockBack());
        }
    }
    void StopBloodSplatter()
    {
        if (bloodParticleSystem != null)
        {
            bloodParticleSystem.Stop();
        }
    }
    void StopPlayerBloodSplatter()
    {
        if (playerPS != null)
        {
            playerPS.Stop();
        }
    }
    IEnumerator KnockBack()
    {

        yield return waitFix;
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);

    }
    ParticleSystem playerPS;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isAlive || !collision.CompareTag("Player")) return;
        if (attackCoundown <= 0)
        {
            SFXPlay.instance.PlayHurt();
            collision.GetComponent<Player>().health -= damage;
            attackCoundown = 0.5f;
            anim.SetFloat("NormalState", 0f);
            anim.SetTrigger("Attack");
            playerPS = collision.GetComponent<ParticleSystem>();
            playerPS.Play();
            Invoke("StopPlayerBloodSplatter", 0.05f);

        }
    }
}
