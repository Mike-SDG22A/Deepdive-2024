using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Button mainmenu;
    public Button retry;

    public float timer = 0f;
    public Text displayText;
    public Text besttime;
    public Canvas timerui;
    public float startTime = 3f;
    public bool finished = false;
    public float fastesttime;
    public RawImage red;
    public RawImage orange;
    public RawImage green;

    void Start()
    {
        Invoke("StartTimer", 3f);
        finished = false;
        LoadFastestTime();
        StartCoroutine(TrafficLightSequence());

    }

    void Update()
    {
        if (Input.GetKey(KeyCode.P)) { finished = true; }



        if (!finished)
        {
            timer += Time.deltaTime;
            displayText.text = timer.ToString("F2");
        }
        if (finished)
        {
            mainmenu.gameObject.SetActive(true);
            retry.gameObject.SetActive(true);
            if (timer < fastesttime)
            {
                fastesttime = timer;
            }
            displayText.text = "curent time: " + timer.ToString("F2");
            besttime.text = "best time " + fastesttime.ToString("F2");

            PlayerPrefs.SetFloat("FastestTime", fastesttime);

        }
    }

    public void StartTimer()
    {
        timerui.gameObject.SetActive(true);
        timer = 0f;
    }
    public void LoadFastestTime()
    {
        if (PlayerPrefs.HasKey("FastestTime"))
        {
            fastesttime = PlayerPrefs.GetFloat("FastestTime");
        }
        else
        {
            fastesttime = float.MaxValue; 
        }
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private System.Collections.IEnumerator TrafficLightSequence()
    {
        red.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);

        red.gameObject.SetActive(false);
        orange.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);

        orange.gameObject.SetActive(false);
        green.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);

        green.gameObject.SetActive(false);

    }
}