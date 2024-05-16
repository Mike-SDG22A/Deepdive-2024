using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ai : MonoBehaviour
{
    public Transform[] checkpoints;  
    private int currentCheckpointIndex = 0; 
    private NavMeshAgent agent; 

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); 
        if (checkpoints.Length > 0)
        {
            agent.SetDestination(checkpoints[currentCheckpointIndex].position);
        }
    }

    void Update()
    {

    }

    void GoToNextCheckpoint()
    {
        currentCheckpointIndex = (currentCheckpointIndex + 1) % checkpoints.Length; 
        agent.SetDestination(checkpoints[currentCheckpointIndex].position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            Debug.Log("test");
            GoToNextCheckpoint();
        }
    }
}
