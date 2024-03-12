using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class WaypointChase : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent agent;
    private Transform playerTransform;
    private bool isChasing = false;
    public GameObject[] waypoints;
    private int currentWaypoint = 0;
    private Animator playerAnimator;

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        // Disable renderers of all waypoints at the start
        foreach (GameObject waypoint in waypoints)
        {
            waypoint.GetComponent<Renderer>().enabled = false;
        }

        SetNextWaypoint();
    }

    void Update()
    {
        if (!isChasing)
        {
            if (waypoints.Length == 0)
                return;

            // If close to the current waypoint, move to the next one
            if (Vector3.Distance(transform.position, waypoints[currentWaypoint].transform.position) < 1f)
            {
                SetNextWaypoint();
            }
        }
        else
        {
            if (playerTransform != null)
            {
                agent.SetDestination(playerTransform.position);
            }
        }

        // Check if Q key is pressed to start chasing
        if (Input.GetKeyDown(KeyCode.Q) && !isChasing)
        {
            StartChasing();
            //SetPlayerIsAttacking(true);
        }
    }

    void SetNextWaypoint()
    {
        currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        agent.SetDestination(waypoints[currentWaypoint].transform.position);
    }

    void StartChasing()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerAnimator = playerTransform.GetComponent<Animator>();
        isChasing = true;
    }

    //    void SetPlayerIsAttacking(bool value)
    //    {
    //        if (playerAnimator != null)
    //        {
    //            playerAnimator.SetBool("IsAttacking", value);
    //        }
    //    }
}