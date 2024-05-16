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
        checkPointCount = allCheckpoints.Count - 1;
        currentCheckpoint = allCheckpoints[checkPointCount];
    }

    /// <summary>
    /// logs the win.
    /// </summary>
    void Update()
    {
        if(LapCount == 1)
        {
            Debug.Log("You Win!");
            timer.finished = true;
        }   
    }

    #endregion unity start/update

    #region checkpointCounters

    /// <summary>
    /// Compares tag to see if you went through a checkpoint.
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)
    {
        {
            // hier wordt een functie aangeroepen
            if (other.CompareTag("Checkpoint") && other.gameObject == currentCheckpoint)
            {
                PlayerCheckpointCounter(other.gameObject);
            }
        }
    }

    /// <summary>
    /// keeps count of the checkpoint.
    /// adds lap after a certain amount of checkpoints.
    /// </summary>
    private void PlayerCheckpointCounter(GameObject obj)
    {
        print("Checkpoint reached");
        if (checkPointCount == 0)
        {
            LapCount++;
            checkPointCount = allCheckpoints.Count - 1; // Reset de checkpoint teller naar het laatste checkpoint
        }
        else
        {
            checkPointCount--;
        }
        currentCheckpoint = allCheckpoints[checkPointCount]; // Update naar het volgende checkpoint
    }
    #endregion checkpointCounters
}