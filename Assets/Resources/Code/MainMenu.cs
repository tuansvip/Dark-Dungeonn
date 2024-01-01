using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public float timer = 0;
    public void QuitGame()
    {
        Application.Quit();
    }
    public void StartGame()
    {
        SFXPlay.instance.PlayClick();
        StartCoroutine(StartCutScene());
    }

    private IEnumerator StartCutScene()
    {
        if (timer <= 0.5f) yield return null;
        SceneManager.LoadScene(1);
    }
}
