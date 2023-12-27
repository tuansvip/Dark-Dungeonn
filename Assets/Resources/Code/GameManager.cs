using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Player player;
    public Room3 room3;
    private void Awake()
    {
        instance = this;
    }



}
