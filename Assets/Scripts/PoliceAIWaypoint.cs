using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class PoliceAIWaypoint : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent agent;
    public GameObject[] waypoints;
    private int currentWaypoint = 0;
    // private bool isChasing = false;
    private Transform playerTransform;
    private float timeSinceLastSeen = 0f;
    private float loseInterestTime = 5f; // Time in seconds after which cop loses interest if player not seen

    // Vision and crime detection
    public float sightRange = 15f;
    // private float eyeOffsetX = 0.2f; // Horizontal offset for eyes from the center
    // private float eyeHeight = 1.6f; // Vertical offset for eyes from the base
    public float attackRange = 10f; // Range within which the cop will attack the player

    // Animation
    private Animator copAnimator; 
    private const string isChasingParam = "IsChasing";
    private const string isAttackingParam = "IsAttacking"; // Assuming there's an 'IsAttacking' animation parameter
    private float nextTurnTime = 0f;
    private bool isTurning = false;

    // Speeds
    public float walkingSpeed = 3f;
    public float runningSpeed = 6f;

    // State
    // private bool isAttacking = false;
    private AJ_controller_Script ajController;
    private ThrowDonutTest throwDonutTestInstance;

    public enum CopState
    {
        Patrol,
        Suspicious,
        Chase,
        Attack,
        Arrest
    }

    public CopState currentState = CopState.Patrol;

    void UpdateState()
    {
        switch (currentState)
        {
            case CopState.Patrol:
                if (CheckForCrime())
                { 
                    currentState = CopState.Chase;
                    timeSinceLastSeen = 0f; 
                }
                break;
            case CopState.Chase:
                float chaseDistance = Vector3.Distance(transform.position, playerTransform.position);
                if (chaseDistance <= attackRange){ currentState = CopState.Attack;}
                else if (timeSinceLastSeen >= loseInterestTime)
                {
                    StopChasing();
                    currentState = CopState.Patrol;
                    timeSinceLastSeen += Time.deltaTime;
                }
                else if (chaseDistance > sightRange)
                {
                    StopChasing();
                    currentState = CopState.Patrol;
                }
                break;
            case CopState.Attack:
                float attackDistance = Vector3.Distance(transform.position, playerTransform.position);
                if (attackDistance > attackRange) currentState = CopState.Chase; // Simplified condition
                break;
            // Add other state transitions here
        }
    }


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

        throwDonutTestInstance = gameObject.AddComponent<ThrowDonutTest>();
    }

    void Update()
    {
        if (currentState == CopState.Chase)
        {
            timeSinceLastSeen += Time.deltaTime;
        }
        UpdateState(); // Determine the current state based on game conditions

        switch (currentState)
        {
            case CopState.Patrol:
                SetNextWaypoint();
                break;
            case CopState.Chase:
                StartChasing();
                break;
            case CopState.Attack:
                HandleAttack();
                break;
            case CopState.Arrest:
                ArrestPlayer();
                break;
            case CopState.Suspicious:
                SuspiciousBehavior();
                break;
        }



        // if (!isAttacking)
        // {
        //     // Continue with other behaviors if not attacking
        //     if (CheckForCrime())
        //     {
        //         if (!isChasing)
        //         {
        //             StartChasing();
        //         }
        //         timeSinceLastSeen = 0f; // Reset timer since player is seen committing a crime
        //     }
        //     else
        //     {
        //         timeSinceLastSeen += Time.deltaTime;
        //         if (isChasing && timeSinceLastSeen >= loseInterestTime)
        //         {
        //             StopChasing();
        //         }
        //     }

        //     if (!isChasing)
        //     {
        //         // PatrolWaypoints();
        //         if (!isTurning && Time.time >= nextTurnTime)
        //         {
        //             isTurning = true;
        //             nextTurnTime = Time.time + 2.30f; // Set next turn time
        //             int rand = Random.Range(0, 11); // Random number between 0 and 10
        //             if (rand == 5)
        //             {
        //                 // Set IsTurning parameter to 5 to trigger turn animation
        //                 copAnimator.SetInteger("IsTurning", rand);
        //                 // Stop chasing waypoint during turn
        //                 agent.ResetPath();
        //             }
        //             else
        //             {
        //                 // Set IsTurning parameter to 0 for no turn
        //                 copAnimator.SetInteger("IsTurning", 0);
        //             }
        //         }
        //         else if (isTurning && Time.time >= nextTurnTime)
        //         {
        //             // Turn finished, resume patrolling
        //             isTurning = false;
        //             SetNextWaypoint();
        //         }
        //     }
        //     else
        //     {
        //         StartChasing();
                
        //     }
        // }
        // else if(isAttacking)
        // {
        //     // Continue with attack
        //     HandleAttack();
        // }
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
                Debug.DrawRay(eyePosition, direction * rayDistance, Color.green); // Visualize the ray in the Scene view
                
                if (hit.collider.CompareTag("Player"))
                {
                    
                    // PlayerState playerState = hit.collider.GetComponent<PlayerState>();
                    if (ajController.currentState == AJ_controller_Script.PlayerState.Spraying)
                    {
                        crimeDetected = true;
                        Debug.DrawRay(eyePosition, direction * rayDistance, Color.red); // Visualize the ray in the Scene view
                        currentState = CopState.Chase;
                        break;
                    }
                }
            }
            else
            {
                Debug.DrawRay(eyePosition, direction * rayDistance, Color.white); // Visualize the ray when no collision occurs
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
        Debug.Log("Chasing Player");
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        // isChasing = true;
        copAnimator.SetBool(isChasingParam, true);
        agent.speed = runningSpeed;
        agent.SetDestination(playerTransform.position);
    }

    void StopChasing()
    {
        playerTransform = null;
        // isChasing = false;
        copAnimator.SetBool(isChasingParam, false);
        agent.speed = walkingSpeed;
        currentState = CopState.Suspicious;
        Debug.Log("Stopped Chasing, Too Many Donuts");
    }

    void SuspiciousBehavior()
    {
        Debug.Log("Looking for Suspicious Behavior");
        if (currentState == CopState.Suspicious)
            {
                if (!isTurning && Time.time >= nextTurnTime)
                {
                    isTurning = true;
                    nextTurnTime = Time.time + 2.30f; // Set next turn time
                    int rand = Random.Range(0, 11); // Random number between 0 and 10
                    if (rand == 5)
                    {
                        // Set IsTurning parameter to 5 to trigger turn animation
                        copAnimator.SetInteger("IsTurning", rand);
                        // Stop chasing waypoint during turn
                        agent.ResetPath();
                    }
                    else
                    {
                        // Set IsTurning parameter to 0 for no turn
                        copAnimator.SetInteger("IsTurning", 0);
                    }
                }
                else if (isTurning && Time.time >= nextTurnTime)
                {
                    // Turn finished, resume patrolling
                    isTurning = false;
                    currentState = CopState.Patrol;
                    SetNextWaypoint();
                    return;
                }
            }
        
        isTurning = false;
        currentState = CopState.Patrol;
        SetNextWaypoint();

    }

    void HandleAttack()
    {
        Debug.Log("Attacking Player");
        // agent.isStopped = true; // Allow the NavMeshAgent to resume moving
        // copAnimator.SetBool(isAttackingParam, true);
        throwDonutTestInstance.ThrowDonut();
        Debug.Log("Donut Thrown");
        // if (Vector3.Distance(transform.position, playerTransform.position) <= attackRange && isChasing)
        // {
        //     // Within attack range and chasing, start attack
        //     if (currentState != CopState.Attack)
        //     {
        //         isAttacking = false;
        //         agent.isStopped = false; // Stop the NavMeshAgent from moving
        //         copAnimator.SetBool(isAttackingParam, false);
        //     }
        // }
        // else
        // {
        //     // Out of attack range or not chasing, stop attack
        //     if (isAttacking)
        //     {
        //         isAttacking = true;
        //         agent.isStopped = true; // Allow the NavMeshAgent to resume moving
        //         copAnimator.SetBool(isAttackingParam, true);
        //         throwDonutTestInstance.ThrowDonut();
        //     }
        // }
    }

    void ArrestPlayer()
    {
       Debug.Log("Player Arrested");
    }
}

