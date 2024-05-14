using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointScript : MonoBehaviour
{
    [Header("Checkpoints variables")]
    public List<GameObject> checkPoints;
    public GameObject currentCheckPoint;
    public int checkPointCounter;
    public List<string> passedCheckpoints = new List<string>();

    /// <summary>
    /// checks if you hit the checkpoint and if you had hit that checkpoint before.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        
    }
}
