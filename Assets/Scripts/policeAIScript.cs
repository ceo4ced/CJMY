using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class policeAIScript : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent navAgent;
    private Transform player;

    private LayerMask groundLayerMask;
    private LayerMask playerLayerMask;
    //behavior variables
    private bool isAttacking = false;
    private float runAnimationSpeed = 5.0f;
    //awareness variables
    private Transform visionOrigin;
    public float sightRange = 15;
    public float attackRange = 2;
    float eyeOffsetX = 0.2f; // Horizontal offset for eyes from the center
    float eyeHeight = 1.6f; // Vertical offset for eyes from the base

    // private bool playerInSightRange = false;
    private bool sawCrime = false;
    private bool playerInAttackRange = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        visionOrigin = transform.Find("VisionOrigin");
        //Debug.Log(visionOrigin.position);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        //Debug.Log(player.position);
        navAgent = GetComponent<NavMeshAgent>();
        playerLayerMask = LayerMask.GetMask("Character");
        groundLayerMask = LayerMask.GetMask("Ground");
        //Debug.Log("Initialized");
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckForCrime())
        {
            //Debug.Log("Player is spraying!");
            sawCrime = true;
        }
        else
        {
            //Debug.Log("Player is not spraying!");
            sawCrime = false;
        }

        if (sawCrime)
        {
            Chasing();
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayerMask);
            if (playerInAttackRange)
            {
                Attacking();
            }
        }
        else
        {
            Roaming();
        }
    }



    // void Update2()
    // {
    //     playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayerMask);
    //     if (playerInSightRange)
    //     {
    //         RaycastHit hit;
    //         Physics.Linecast(visionOrigin.position, new Vector3(player.position.x, player.position.y + .5f, player.position.z), out hit);
    //         Debug.DrawLine(visionOrigin.position, new Vector3(player.position.x, player.position.y + .5f, player.position.z));
    //         Debug.Log(hit.point);
    //         Debug.Log(hit.collider.gameObject.tag);
    //         if (!sawPlayer)
    //         {
    //             sawPlayer = hit.collider.gameObject.CompareTag("Player");
    //         }
    //         Debug.Log(sawPlayer);

    //     }
    //     else
    //     {
    //         sawPlayer = false;
    //     }
    //     playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayerMask);

    //     if (playerInAttackRange)
    //     {
    //         Attacking();
    //     }
    //     else if (sawPlayer)
    //     {
    //         Chasing();
    //     }
    //     else
    //     {
    //         Roaming();
    //     }
    // }


    private void Roaming()
    {
        isAttacking = false;
        navAgent.ResetPath();
        animator.SetBool("isAttacking", isAttacking);
        animator.SetBool("isChasing", false);
        animator.SetFloat("Speed", 0); //change this to 1 when we implement actual roaming/waypoints, and maybe a new idle state for where it is 0 
    }
    private void Chasing()
    {
        isAttacking = false;
        navAgent.SetDestination(player.position);
        animator.SetBool("isAttacking", isAttacking);
        animator.SetBool("isChasing", true);
        animator.SetFloat("Speed", runAnimationSpeed);
    }
    private void Attacking()
    {
        navAgent.ResetPath();
        isAttacking = true;
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        animator.SetBool("isAttacking", isAttacking);
        animator.SetBool("isChasing", false);
        animator.SetFloat("Speed", 0);
    }

    bool CheckForCrime()
    {
        Vector3 leftEyePosition = transform.position + transform.right * -eyeOffsetX + Vector3.up * eyeHeight;
        Vector3 rightEyePosition = transform.position + transform.right * eyeOffsetX + Vector3.up * eyeHeight;
        Vector3 sightDirection = transform.TransformDirection(Vector3.forward) * sightRange;

        RaycastHit hit;
        // Perform raycasts from both "eyes"
        if (Physics.Raycast(leftEyePosition, sightDirection, out hit, sightRange) || Physics.Raycast(rightEyePosition, sightDirection, out hit, sightRange))
        {
            if (hit.collider.CompareTag("Player"))
            {
                PlayerState playerState = hit.collider.GetComponent<PlayerState>();
                // Check if the player is in the spraying state
                if (playerState != null && playerState.currentState == PlayerState.State.Spraying)
                {
                    //Debug.Log("Cop sees the player spraying!");
                    // currentState = CopState.Chase;
                    return true;
                }
            }
        }
        Debug.DrawRay(leftEyePosition, sightDirection, Color.red);
        Debug.DrawRay(rightEyePosition, sightDirection, Color.blue);

        return false;
    }


}