using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class PoliceAIWaypoint : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent agent;
    public GameObject[] waypoints;
    private int currentWaypoint = 0;
    private bool isChasing = false;
    private Transform playerTransform;
    private float timeSinceLastSeen = 0f;
    private float loseInterestTime = 5f; // Time in seconds after which cop loses interest if player not seen

    // Vision and crime detection
    public float sightRange = 15f;
    // private float eyeOffsetX = 0.2f; // Horizontal offset for eyes from the center
    // private float eyeHeight = 1.6f; // Vertical offset for eyes from the base
    public float attackRange = 2f; // Range within which the cop will attack the player

    // Animation
    private Animator copAnimator; 
    private const string isChasingParam = "IsChasing";
    private const string isAttackingParam = "IsAttacking"; // Assuming there's an 'IsAttacking' animation parameter

    // Speeds
    public float walkingSpeed = 3f;
    public float runningSpeed = 6f;

    // State
    private bool isAttacking = false;
    private AJ_controller_Script ajController;

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        // Find the GameObject with the "Cop" tag
        GameObject copCharacter = GameObject.FindGameObjectWithTag("Cop");

        ajController = GameObject.FindGameObjectWithTag("Player").GetComponent<AJ_controller_Script>();

        // Get the Animator component from the Cop GameObject
        copAnimator = copCharacter.GetComponent<Animator>();

        foreach (GameObject waypoint in waypoints)
        {
            waypoint.GetComponent<Renderer>().enabled = false;
        }

        SetNextWaypoint();

        agent.speed = walkingSpeed;
    }

    void Update()
    {
        // Check for and handle attack
        // HandleAttack();

        if (!isAttacking)
        {
            // Continue with other behaviors if not attacking
            if (CheckForCrime())
            {
                if (!isChasing)
                {
                    StartChasing();
                }
                timeSinceLastSeen = 0f; // Reset timer since player is seen committing a crime
            }
            else
            {
                timeSinceLastSeen += Time.deltaTime;
                if (isChasing && timeSinceLastSeen >= loseInterestTime)
                {
                    StopChasing();
                }
            }

            if (!isChasing)
            {
                // PatrolWaypoints();
                SetNextWaypoint();
            }
            else
            {
                ChasePlayer();
            }
        }
    }

    bool CheckForCrime()
    {
        float eyeHeight = 1.6f; // Height of the eyes from the base
        int numberOfRays = 10; // Total number of rays to cast within the field of view
        float fieldOfView = 90f; // Total angle of the field of view
        float rayDistance = sightRange; // How far each ray can see

        Vector3 eyePosition = transform.position + Vector3.up * eyeHeight; // Central position of the eyes
        bool crimeDetected = false;

        for (int i = 0; i < numberOfRays; i++)
        {
            // Calculate the rotation angle for each ray
            float angle = Mathf.Lerp(-fieldOfView / 2, fieldOfView / 2, (float)i / (numberOfRays - 1));
            Vector3 direction = Quaternion.Euler(0, angle, 0) * transform.forward;

            RaycastHit hit;
            if (Physics.Raycast(eyePosition, direction, out hit, rayDistance))
            {
                Debug.DrawRay(eyePosition, direction * rayDistance, Color.red); // Visualize the ray in the Scene view
                
                if (hit.collider.CompareTag("Player"))
                {
                    
                    // PlayerState playerState = hit.collider.GetComponent<PlayerState>();
                    if (ajController.currentState == AJ_controller_Script.PlayerState.Spraying)
                    {
                        crimeDetected = true;
                        break; // Stop checking if a crime is detected
                    }
                }
            }
            else
            {
                Debug.DrawRay(eyePosition, direction * rayDistance, Color.green); // Visualize the ray when no collision occurs
            }
        }

        return crimeDetected;
    }


    void SetNextWaypoint()
    {
        if (Vector3.Distance(transform.position, waypoints[currentWaypoint].transform.position) < 1f)
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        }

        agent.SetDestination(waypoints[currentWaypoint].transform.position);

        // Set speed back to walking speed when reaching a waypoint
        agent.speed = walkingSpeed;

        copAnimator.SetBool(isChasingParam, false);
    }

    void StartChasing()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        isChasing = true;
        copAnimator.SetBool(isChasingParam, true);
    }

    void StopChasing()
    {
        playerTransform = null;
        isChasing = false;
        copAnimator.SetBool(isChasingParam, false);
        agent.speed = walkingSpeed;
    }

    void ChasePlayer()
    {
        agent.SetDestination(playerTransform.position);
        agent.speed = runningSpeed;
    }

    void HandleAttack()
    {
        if (Vector3.Distance(transform.position, playerTransform.position) <= attackRange && isChasing)
        {
            // Within attack range and chasing, start attack
            if (!isAttacking)
            {
                isAttacking = true;
                agent.isStopped = true; // Stop the NavMeshAgent from moving
                copAnimator.SetBool(isAttackingParam, true);
            }
        }
        else
        {
            // Out of attack range or not chasing, stop attack
            if (isAttacking)
            {
                isAttacking = false;
                agent.isStopped = false; // Allow the NavMeshAgent to resume moving
                copAnimator.SetBool(isAttackingParam, false);
            }
        }
    }
}

