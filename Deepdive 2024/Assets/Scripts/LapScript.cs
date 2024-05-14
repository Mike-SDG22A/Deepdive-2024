using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapScript : MonoBehaviour
{
    public int checkPointCount = 0;
    private int requiredCheckpoints;
    public int LapCount = 0;

    void Update()
    {
        if(LapCount == 3)
        {
            Debug.Log("You Win!");
        }   
    }

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

    private void PlayerCheckpointCounter()
    {
        checkPointCount++;
        if (checkPointCount > requiredCheckpoints)
        {
            if (checkPointCount == 48)
            {
                LapCount++;
                if (LapCount > 0)
                {
                    checkPointCount = 0;
                }
            }
        }
    }
}
