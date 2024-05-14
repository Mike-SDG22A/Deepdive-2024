using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float timer = 0f;
    public Text displayText;
    public Canvas timerui;
    public float startTime = 3f;

    void Start()
    {
        Invoke("StartTimer", 3f);
    }

    void Update()
    {


        timer = Time.time - startTime;

        displayText.text = "Timer: " + timer.ToString("F2");
    }

    public void StartTimer()
    {
        timerui.gameObject.SetActive(true);
        timer = 0f;

    }

}