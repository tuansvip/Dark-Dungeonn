using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    public float timer;
    private void Awake()
    {
        timer = 0;
    }
    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (timer >= 123f)
        {
            OnSkip();
        }
    }
    private void OnSkip()
    {
        SceneManager.LoadScene(0);
    }
}
