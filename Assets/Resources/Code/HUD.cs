using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType { Health, ArrowDamage, MeleeDamage, Room3Timer, BossHealth }
    public InfoType type;

    Text myText;
    Slider mySlider;
    private void Awake()
    {
        myText= GetComponent<Text>();
        mySlider = GetComponent<Slider>();
    }

    private void LateUpdate()
    {
        switch (type)
        {
            case InfoType.Health:
                float currHealth = GameManager.instance.player.health;
                float maxHealth = GameManager.instance.player.maxHealth;
                mySlider.value = currHealth/maxHealth;
                break;
            case InfoType.ArrowDamage:
                myText.text = string.Format("Arrow Damage: {0:0.##}",  GameManager.instance.player.arrowDmg);
                break;
            case InfoType.MeleeDamage:
                myText.text = string.Format("Melee Damage: {0:0.##}", GameManager.instance.player.meleeDmg);
                break;
            case InfoType.Room3Timer:
                int second = (int)GameManager.instance.room3.timer;
                int milisecond = (int)((GameManager.instance.room3.timer - second) * 100);
                myText.text = string.Format("Sống sót trong {0}:{1}\ts!",second,milisecond );
                break;
            case InfoType.BossHealth:
                float currBossHealth = GameManager.instance.boss.health;
                float maxBossHealth = GameManager.instance.boss.maxHealth;
                mySlider.value = currBossHealth / maxBossHealth;
                break;
        }
    }
}
