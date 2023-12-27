using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckClear : MonoBehaviour
{
    public bool isClear;
    private void FixedUpdate()
    {
        if (isClear) return;
        Enemy[] enemy = GetComponentsInChildren<Enemy>();
        foreach (Enemy enem in enemy)
        {
            isClear = true;
            if (enem.isAlive)
            {
                isClear = false;
                break;
            }
        }
    }
}
