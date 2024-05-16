using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float timer = 0f;
    public TMP_Text timerText;
    public Canvas canvas;
    public GameObject gui;
    public GameObject endScreen;
    public float startTime = 3f;
    public bool finished = false;
    public float fastesttime;
    public RawImage red;
    public RawImage orange;
    public RawImage green;

    [SerializeField] CarController player;

    [SerializeField] TMP_Text rpmText;
    [SerializeField] TMP_Text speedText;
    Leaderboard leaderboard;
    bool onlyOnce = false;

    void Start()
    {
        leaderboard = FindObjectOfType<Leaderboard>();
        //Invoke("StartTimer", 3f);
        finished = false;
        //LoadFastestTime();
        //StartCoroutine(TrafficLightSequence());

    }

    void Update()
    {
       // if (Input.GetKey(KeyCode.P)) { finished = true; }

        speedText.text = player.kmp.ToString("F1") + " kph";
        rpmText.text = player.rmpCounter.ToString("F0") + " rpm";

        if (!finished)
        {

            timer += Time.deltaTime;

            int min = (int)Mathf.Floor(timer / 60);

            float tempTimer = timer - (60 * min);

            string extraNumSec = "";
            string extraNumMin = "";
            if (tempTimer < 10) extraNumSec = "0";
            if (min < 10) extraNumMin = "0";

            string timeString = $"{extraNumMin}{min}.{extraNumSec}{tempTimer.ToString("F2")}".Replace('.', ':');

            timerText.text = timeString;
        }
        if (finished && !onlyOnce)
        {
            onlyOnce = true;
            gui.SetActive(false);
            endScreen.SetActive(true);
            leaderboard.AddToBoard(new Board("---", timer));
        }
    }

    public void StartTimer()
    {
        canvas.gameObject.SetActive(true);
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