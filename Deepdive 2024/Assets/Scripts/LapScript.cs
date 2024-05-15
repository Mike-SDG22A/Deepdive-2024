using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapScript : MonoBehaviour
{
    #region checkpoint variables

    public List<GameObject> allCheckpoints;
    [SerializeField] GameObject currentCheckpoint;
    [SerializeField] Transform checkpointHolder;
    public int checkPointCount = 0;
    private int requiredCheckpoints = 5;
    public int LapCount = 0;
    public Timer timer;

    #endregion checkpoint variables

    #region unity start/update

    /// <summary>
    /// prevents you from going through a checkpoint twice in the current lap
    /// </summary>
    void Start()
    {
        for(int i = 0; i < checkpointHolder.childCount; i++)
        {
            if (!allCheckpoints.Contains(checkpointHolder.GetChild(i).gameObject)&&
                checkpointHolder.GetChild(i) != checkpointHolder)
            {
                allCheckpoints.Add(checkpointHolder.GetChild(i).gameObject);
            }
        }   
    }

    /// <summary>
    /// logs the win.
    /// </summary>
    void Update()
    {
        if(LapCount == 3)
        {
            Debug.Log("You Win!");
            timer.isTimerActive = false;
        }   
    }

    #endregion unity start/update

    /*
    void Win()
    {
        switch (LapCount)
        {
            case 0:
                break;
        }
    }
    */

    #region checkpointCounters

    /// <summary>
    /// Compares tag to see if you went through a checkpoint.
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)
    {
        {
            // hier wordt een functie aangeroepen
            if (other.CompareTag("Checkpoint") && this.CompareTag("Player"))
            {
                PlayerCheckpointCounter();
            }
        }
    }

    /// <summary>
    /// telt de checkpoint op.
    /// moet na een bepaald hoeveelheid checkpoints de lap optellen.
    /// </summary>
    private void PlayerCheckpointCounter()
    {
        checkPointCount++;
        if (checkPointCount > allCheckpoints.Count)
        {
            if (checkPointCount == allCheckpoints.Count - 1)
            {
                LapCount++;
                if (LapCount > 0)
                {
                    checkPointCount = 0;
                }
            }
        }
    }

    #endregion checkpointCounters
}