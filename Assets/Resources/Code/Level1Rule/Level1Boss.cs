using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.GridBrushBase;

public class Level1Boss : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public float damage;
    public float speed = 1.3f;
    public Transform target;
    public float timer;
    public float skillCoundown;
    public float skillTimer;
    public float changeDirectionInterval = 1f;
    public Vector2 ranDir;
    public Vector2 moveDir;
    public Room4Pool pool;
    public bool isAlive;
    public float attackCoundown;
    public ParticleSystem bloodParticleSystem;

    Collider2D coll;
    Rigidbody2D rigid;
    Animator anim;

    private void Awake()
    {
        SetRanDir();
        isAlive = true;
        anim = GetComponentInChildren<Animator>();
        bloodParticleSystem = GetComponent<ParticleSystem>();
        coll = GetComponent<Collider2D>();
        rigid = GetComponent<Rigidbody2D>();
        health = maxHealth;
    }

    private void SetRanDir()
    {
        ranDir = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
    }

    private void FixedUpdate()
    {
        if (!isAlive) return;
        attackCoundown -= Time.fixedDeltaTime;
        skillCoundown = UnityEngine.Random.Range(1.3f, 4f);
        skillTimer -= Time.fixedDeltaTime;
        timer -= Time.fixedDeltaTime;
        transform.Translate(moveDir * Time.fixedDeltaTime);
        if (skillTimer <= 0f)
        {
            int selectSkills = UnityEngine.Random.Range(1, 4);
            skillTimer = skillCoundown;
            Debug.Log(selectSkills);
            switch (selectSkills)
            {
                case 1:
                    RunTowardPlayer(); 
                    timer = 1f;
                    return;
                case 2:
                    FireBall();
                    return;
                case 3:
                    Summon();
                    return;
            }
        }
        if (timer <= 0f)
        {
            moveDir = ranDir.normalized * speed;
            if (timer <= 0f)
            {
                SetRanDir();
                timer = changeDirectionInterval;
            }
        }
    }
    private void LateUpdate()
    {
        if (moveDir != Vector2.zero)
            anim.SetFloat("RunState", 0.25f);
        else anim.SetFloat("RunState", 0f);
        if (moveDir.x != 0)
        {
            if (moveDir.x < 0)
            {
                transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
            }
            else
            {
                transform.localScale = new Vector3(-2.5f, 2.5f, 2.5f);
            }
        }
    }

    private void Summon()
    {
        anim.SetFloat("AttackState", 0f);
        anim.SetFloat("NormalState", 0.4946043f);
        anim.SetTrigger("Attack");
        StartCoroutine(SummonAfterAnimation());

    }

    private IEnumerator SummonAfterAnimation()
    {
        yield return null;
        while (skillCoundown - skillTimer < 0.8f)
        {
            yield return null;
        }
        GameObject enemy = pool.Get(1);
        enemy.transform.position = transform.position;
    }

    private void FireBall()
    {
        anim.SetFloat("AttackState", 0f);
        anim.SetFloat("NormalState", 0.4946043f);
        anim.SetTrigger("Attack");
        StartCoroutine(FireAfterAnimation());
    }

    private IEnumerator FireAfterAnimation()
    {
        yield return null;
        while (skillCoundown - skillTimer < 0.8f)
        {
            yield return null;
        }
        SFXPlay.instance.PlayFireball();
        Vector3 dir = target.position - transform.position;
        dir.z = 0f;
        dir = dir.normalized;
        Transform fireball = pool.Get(0).transform;
        Vector3 tempPos = transform.position;
        tempPos.y += 0.2f;
        fireball.position = tempPos;
        fireball.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        fireball.GetComponent<FireBall>().Init(15, 0, dir);
    }

    private void RunTowardPlayer()
    {
        Vector2 dir = ((Vector2)target.position -  (Vector2)transform.position);
        moveDir = dir.normalized * 10f;
    }
    ParticleSystem playerPS;


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
            coll.enabled = false;
            rigid.simulated = false;
            isAlive = false;
        }

    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (attackCoundown <= 0 && collision.collider.CompareTag("Player"))
        {
            SFXPlay.instance.PlayHurt();
            collision.collider.GetComponent<Player>().health -= damage;
            attackCoundown = 0.3f;
            playerPS = collision.collider.GetComponent<ParticleSystem>();
            playerPS.Play();
            Invoke("StopPlayerBloodSplatter", 0.05f);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

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
}
