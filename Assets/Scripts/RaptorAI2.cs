using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class RaptorAI2 : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent navAgent;
    private Transform player;

    private LayerMask groundLayerMask;
    private LayerMask playerLayerMask;
    //behavior variables
    private bool isAttacking = false;
    //awareness variables
    private Transform visionOrigin;
    private float sightRange = 15;
    private float attackRange = 2;
    private bool playerInSightRange = false;
    private bool sawPlayer = false;
    private bool playerInAttackRange = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        visionOrigin = transform.Find("VisionOrigin");
        Debug.Log(visionOrigin.position);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        Debug.Log(player.position);
        navAgent = GetComponent<NavMeshAgent>();
        playerLayerMask = LayerMask.GetMask("Character");
        groundLayerMask = LayerMask.GetMask("Ground");
        Debug.Log("Initialized");
    }

    // Update is called once per frame
    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayerMask);
        if (playerInSightRange)
        {
            RaycastHit hit;
            Physics.Linecast(visionOrigin.position, new Vector3(player.position.x, player.position.y + .5f, player.position.z), out hit);
            Debug.DrawLine(visionOrigin.position, new Vector3(player.position.x, player.position.y + .5f, player.position.z));
            Debug.Log(hit.point);
            Debug.Log(hit.collider.gameObject.tag);
            if (!sawPlayer)
            {
                sawPlayer = hit.collider.gameObject.CompareTag("Player");
            }
            Debug.Log(sawPlayer);

        }
        else
        {
            sawPlayer = false;
        }
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayerMask);

        if (playerInAttackRange)
        {
            Attacking();
        }
        else if (sawPlayer)
        {
            Chasing();
        }
        else
        {
            Roaming();
        }
    }


    private void Roaming()
    {
        isAttacking = false;
        navAgent.ResetPath();
        animator.SetBool("isAttacking", isAttacking);
        animator.SetBool("isChasing", false);
    }
    private void Chasing()
    {
        isAttacking = false;
        navAgent.SetDestination(player.position);
        animator.SetBool("isAttacking", isAttacking);
        animator.SetBool("isChasing", true);
    }
    private void Attacking()
    {
        navAgent.ResetPath();
        isAttacking = true;
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        animator.SetBool("isAttacking", isAttacking);
        animator.SetBool("isChasing", false);
    }

}