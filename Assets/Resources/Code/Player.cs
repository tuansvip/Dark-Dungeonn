using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public static Player instance;

    [Header("# Stats")]
    public float health = 100;
    public float maxHealth =100;
    public float speed;
    public bool isLive;

    [Header("# Arrow")]
    public float arrowDmg;
    public float baseArrowDmg;
    public float arrowSpd;

    [Header("# Melee")]
    public float baseMeleeDmg;
    public float meleeDmg;
    public float meleeSpd;

    [Header("# Vector")]
    public Vector2 inputVec2;
    public Vector3 playerDir;

    [Header("# GameObject")]
    public PoolManager pool;

    [Header("# Others")]
    public float gameTime;

    float lastTimeArrow;
    float lastTimeMelee;
    Animator anim;
    Rigidbody2D rigid;
    Transform transform;
    Scanner scanner;
    PlayerInput playerInput;
    private void Start()
    {
        Time.timeScale = 1f;
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        transform = GetComponent<Transform>();
        anim = GetComponentInChildren<Animator>();
        scanner = GetComponent<Scanner>();
        gameTime = 0;
        lastTimeArrow = -arrowSpd;
        lastTimeMelee = -meleeSpd;
        health = maxHealth;
        isLive = true;
    }

    private void OnMove(InputValue input)
    {
        if (!isLive) return;
        inputVec2 = input.Get<Vector2>();
    }
    private void OnFire()
    {
        if (!isLive) return;

        if (gameTime - lastTimeArrow < arrowSpd) 
        {
            return;
        }
        lastTimeArrow = gameTime;
        anim.SetFloat("AttackState", 0f);
        anim.SetFloat("NormalState", 0.4946043f);
        anim.SetTrigger("Attack");
        StartCoroutine(FireAfterAnimation());

    }
    private IEnumerator FireAfterAnimation()
    {
        yield return null;
        while (gameTime - lastTimeArrow < 0.6f)
        {
            yield return null;
        }
        SFXPlay.instance.PlayArrow();
        Vector3 dir;

        if (!scanner.nearestTarget)
        {
            dir = playerDir;
        }
        else
        {
            Vector3 targetPos = scanner.nearestTarget.position;
            dir = targetPos - transform.position;
        }

        dir = dir.normalized;
        Transform arrow = pool.Get(0).transform;
        Vector3 tempPos = transform.position;
        tempPos.y += 0.2f;
        arrow.position = tempPos;
        arrow.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        arrow.GetComponent<Arrow>().Init(arrowDmg, dir);
    }
    private void OnMeleeAtk()
    {
        if (!isLive) return;

        if (gameTime - lastTimeMelee < meleeSpd)
        {
            return;
        }

        lastTimeMelee = gameTime;
        anim.SetFloat("AttackState", 0f);
        anim.SetFloat("NormalState", 0f);
        anim.SetTrigger("Attack");

        StartCoroutine(MeleeAfterAnimation());

    }
    private IEnumerator MeleeAfterAnimation()
    {

        yield return null;
        while(gameTime - lastTimeMelee < 0.2f)
        {
            yield return null;
        }
        SFXPlay.instance.PlaySword();
        Vector3 dir;
        dir = playerDir.normalized;
        Transform melee = pool.Get(1).transform;
        Vector3 tempPos = transform.position;
        tempPos.y += 0.2f;
        melee.position = tempPos;
        melee.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        melee.GetComponent<Melee>().Init(meleeDmg, dir);
    }
    private void OnEsc()
    {
        if (GameManager.instance.isPause)
        {
            GameManager.instance.Resume();
        }
        else
        {
            GameManager.instance.Pause();
        }
    }


    private void FixedUpdate()
    {
        gameTime += Time.fixedDeltaTime;
        Vector2 nextVec2 = inputVec2 * speed * Time.fixedDeltaTime;
        if (nextVec2 != Vector2.zero)
        {
            playerDir = new Vector3(nextVec2.x, nextVec2.y, 0f);
        }
        rigid.MovePosition(rigid.position + nextVec2);
        if (health <= 0)
        {
            health = 0;
            anim.SetTrigger("Die");
            rigid.constraints = RigidbodyConstraints2D.FreezeAll;
            rigid.simulated = false;
            GetComponent<Collider2D>().enabled = false;
            time = gameTime;
            SFXPlay.instance.PlayLose();
            StartCoroutine(Die());
        }
    }
    float time;
    IEnumerator Die()
    {
        yield return null; 
        while (gameTime -  time > 2f){
            yield return null;
        }
        isLive = false;
    }

    private void LateUpdate()
    {
        if (inputVec2 != Vector2.zero)
            anim.SetFloat("RunState", 0.25f);
        else anim.SetFloat("RunState", 0f);
        if (inputVec2.x != 0)
        {
            if (inputVec2.x < 0)
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
        if (collision.CompareTag("PowerPotion"))
        {
            arrowDmg += baseArrowDmg * 0.1f;
            meleeDmg += baseMeleeDmg * 0.1f;
            collision.gameObject.SetActive(false);
        }
        if (collision.CompareTag("HealthPotion"))
        {
            if (!isLive || health == maxHealth)
                return;
            health += 10;
            if (health > maxHealth) health = maxHealth;
            collision.gameObject.SetActive(false);
        }
        if(collision.CompareTag("Portal"))
        {
            SFXPlay.instance.PlayWin();
            rigid.constraints = RigidbodyConstraints2D.FreezeAll;
            rigid.simulated = false;
            GetComponent<Collider2D>().enabled = false;
            GameManager.instance.Win();

        }
        
    }
}
