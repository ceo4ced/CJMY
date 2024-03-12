using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutonomousMovement : MonoBehaviour
{
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float detectionRange = 30f;

    private Animator animator;
    private Transform player; // Reference to the player object

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform; // Find the player object by tag
    }

    void Update()
    {
        if (player == null) // Check if player is null (for safety)
        {
            Debug.LogWarning("Player not found!");
            return;
        }

        // Calculate the distance between the character and the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Determine movement speed based on the distance to the player
        float moveSpeed = distanceToPlayer > detectionRange ? walkSpeed : runSpeed;

        // Update animator parameters based on movement speed
        if (moveSpeed > walkSpeed)
        {
            animator.SetFloat("Speed", 6); // Trigger run animation
        }
        else
        {
            animator.SetFloat("Speed", 3); // Trigger walk animation
        }

        // Move the character forward continuously
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }
}