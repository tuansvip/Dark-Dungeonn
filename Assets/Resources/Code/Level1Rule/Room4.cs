using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room4 : MonoBehaviour
{
    public bool isClear;
    public GameObject portal;
    public GameObject bossHUD;
    private void Awake()
    {
        portal = transform.GetChild(3).gameObject;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isClear || !collision.CompareTag("Player")) return;

        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(true);
        bossHUD.SetActive(true);
    }
    private void FixedUpdate()
    {
        if (isClear)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(false);
            StartCoroutine(ShowPortal());

        }

        if (!transform.GetChild(1).GetComponent<Level1Boss>().isAlive)
        {
            bossHUD.SetActive(false);
            isClear = true;
        }
    }

    private IEnumerator ShowPortal()
    {
        portal.SetActive(true);
        Renderer portalRenderer = portal.GetComponent<Renderer>();
        CanvasGroup canvasGroup = portal.GetComponent<CanvasGroup>();
        if (portalRenderer != null)
        {
            Color startColor = portalRenderer.material.color;
            Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 1f);
            float elapsedTime = 0f;
            while(elapsedTime < 1f)
            {
                float alpha = Mathf.Lerp(0f, 1f, elapsedTime / 1f);
                portalRenderer.material.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            portalRenderer.material.color = targetColor;
        }
        else if(canvasGroup != null) { 
            float elapsedTime = 0f;
            while(elapsedTime < 1f) {
                float alpha = Mathf.Lerp(0f, 1f, elapsedTime / 1f);
                canvasGroup.alpha = alpha;
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            canvasGroup.alpha = 1f;
        }
        else
        {
            Debug.Log("error");
        }
    }
}
