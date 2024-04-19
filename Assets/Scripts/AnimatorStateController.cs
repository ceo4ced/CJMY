using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorStateController : MonoBehaviour
{
    public PoliceAIWaypoint policeAI;
    public AJ_controller_Script ajController;
    public Transform playerTransform; // Assign this from the editor or find dynamically
    private Animator animator;
    private AudioSource audioSource;

    public AudioClip vic1Sound;
    public AudioClip vic2Sound;
    public AudioClip vic3Sound;

    private PoliceAIWaypoint.CopState previousState;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if (policeAI == null)
        {
            //Debug.LogError("PoliceAIWaypoint component is not assigned!");
        }

        previousState = policeAI.GetCurrentState(); // Initialize previousState at start
    }

    void Update()
    {
        if (policeAI != null)
        {
            PoliceAIWaypoint.CopState currentState = policeAI.GetCurrentState();
            if (currentState != previousState)
            {
                UpdateAnimatorParameters(currentState);
                previousState = currentState; // Update previousState after processing changes
            }
        }

        if (IsNearestToPlayer())
        {
            audioSource.enabled = true;
        }
        else
        {
            audioSource.enabled = false;
            audioSource.Stop(); // Stop playing if this cop is not the nearest
        }
    }

    bool IsNearestToPlayer()
    {
        float minDistance = float.MaxValue;
        GameObject[] allCops = GameObject.FindGameObjectsWithTag("Cop"); // Assuming all cops have the tag "Cop"
        foreach (var cop in allCops)
        {
            float distance = Vector3.Distance(cop.transform.position, playerTransform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
            }
        }
        // Check if this cop is the nearest
        return Vector3.Distance(transform.position, playerTransform.position) == minDistance;
    }

    void UpdateAnimatorParameters(PoliceAIWaypoint.CopState state)
    {
        switch (state)
        {
            case PoliceAIWaypoint.CopState.Patrol:
                animator.SetBool("IsChasing", false);
                animator.SetBool("IsAttacking", false);
                animator.SetBool("IsTurning", false);
                break;
            case PoliceAIWaypoint.CopState.Suspicious:
                animator.SetBool("IsTurning", true);
                break;
            case PoliceAIWaypoint.CopState.Chase:
                animator.SetBool("IsChasing", true);
                animator.SetBool("IsTurning", false);
                break;
            case PoliceAIWaypoint.CopState.Attack:
                animator.SetBool("IsChasing", true);
                animator.SetBool("IsAttacking", true);
                animator.SetBool("IsTurning", false);
                break;
            case PoliceAIWaypoint.CopState.Arrest:
                if (previousState != PoliceAIWaypoint.CopState.Arrest) // Check if we just entered Arrest state
                {
                    ResetVicBools();
                    SetRandomVicTrue();
                }
                break;
            default:
                break;
        }
    }

    void ResetVicBools()
    {
        animator.SetBool("vic1", false);
        animator.SetBool("vic2", false);
        animator.SetBool("vic3", false);
    }

    void SetRandomVicTrue()
    {
        int randomChoice = Random.Range(1, 4);
        switch (randomChoice)
        {
            case 1:
                animator.SetBool("vic1", true);
                audioSource.clip = vic1Sound;
                break;
            case 2:
                animator.SetBool("vic2", true);
                audioSource.clip = vic2Sound;
                break;
            case 3:
                animator.SetBool("vic3", true);
                audioSource.clip = vic3Sound;
                break;
        }
        audioSource.loop = true;
        audioSource.Play();
    }
}
