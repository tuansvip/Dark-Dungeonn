using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlay : MonoBehaviour
{
    public static SFXPlay instance;
    public AudioSource arrow;
    public AudioSource sword;
    public AudioSource fireball;
    public AudioSource click;
    public AudioSource hit;
    public AudioSource bgMusic;
    public AudioSource win;
    public AudioSource lose;
    public AudioSource hurt;
    public AudioSource burn;


    private void Awake()
    {
        instance = this;
    }
    public void PlayArrow()
    {
        arrow.Play();
    }
    public void PlaySword()
    {
        sword.Play();
    }
    public void PlayClick()
    {
        click.Play();
    }
    public void PlayHit()
    {
        hit.Play();
    }
    public void PlayBgMusic()
    {
        bgMusic.Play();
    }
    public void PlayWin()
    {
        win.Play();
    }
    public void PlayLose()
    {
        lose.Play();
    }
    public void PlayHurt()
    {
        hurt.Play();
    }
    public void PlayFireball()
    {
        fireball.Play();
    }
    public void PlayBurn()
    {
        burn.Play();
    }
}
