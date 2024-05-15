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
        currentCheckpoint = allCheckpoints[0];
    }

    /// <summary>
    /// logs the win.
    /// </summary>
    void Update()
    {
        if(LapCount == 3)
        {
            Debug.Log("You Win!");
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
                PlayerCheckpointCounter();
            }
        }
    }

    /// <summary>
    /// keeps count of the checkpoint.
    /// adds lap after a certain amount of checkpoints.
    /// </summary>
    private void PlayerCheckpointCounter()
    {
        print("Hello");
        checkPointCount++;
        if (checkPointCount >= allCheckpoints.Count)
        {
            LapCount++;
            checkPointCount = 0;
        }
        currentCheckpoint = allCheckpoints[checkPointCount];
    }
    #endregion checkpointCounters
}