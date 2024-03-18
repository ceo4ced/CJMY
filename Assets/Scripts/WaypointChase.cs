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
    private Animator copAnimator; // Animator component of the character with IsChasing parameter
    private AJ_controller_Script ajController; // Reference to AJ_controller_Script

    // Define the name of the boolean parameter in the Animator controller
    private const string isChasingParam = "IsChasing";

    // Define speeds for walking and running
    public float walkingSpeed = 3f;
    public float runningSpeed = 6f;

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        // Find the GameObject with the "Cop" tag
        GameObject copCharacter = GameObject.FindGameObjectWithTag("Cop");

        // Get the Animator component from the Cop GameObject
        copAnimator = copCharacter.GetComponent<Animator>();


        // Disable renderers of all waypoints at the start
        foreach (GameObject waypoint in waypoints)
        {
            waypoint.GetComponent<Renderer>().enabled = false;
        }

        SetNextWaypoint();

        // Get reference to AJ_controller_Script
        ajController = GameObject.FindGameObjectWithTag("Player").GetComponent<AJ_controller_Script>();

        // Set initial speed to walking speed
        agent.speed = walkingSpeed;
    }

    void Update()
    {
        // Check if the player is in the spraying state to start chasing
        if (ajController.currentState == AJ_controller_Script.PlayerState.Spraying && !isChasing)
        {
            StartChasing();
        }

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

                // Change speed to running speed when chasing
                agent.speed = runningSpeed;
            }
        }
    }

    void SetNextWaypoint()
    {
        currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        agent.SetDestination(waypoints[currentWaypoint].transform.position);

        // Set speed back to walking speed when reaching a waypoint
        agent.speed = walkingSpeed;
    }

    void StartChasing()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        isChasing = true;

        // Set the boolean parameter in the Animator controller to true
        copAnimator.SetBool(isChasingParam, true);
    }
}
