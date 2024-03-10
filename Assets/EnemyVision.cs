using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using PlayerState;
public class EnemyVision : MonoBehaviour
{
    public enum CopState
    {
        Patrol,
        Chase
    }

    public CopState currentState = CopState.Patrol;

    public float sightRange = 10f;
    float eyeOffsetX = 0.2f; // Horizontal offset for eyes from the center
    float eyeHeight = 1.6f; // Vertical offset for eyes from the base

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case CopState.Patrol:
                PatrolBehavior();
                CheckForPlayer1();
                break;
            case CopState.Chase:
                ChaseBehavior();
                break;
        }
    }

    void PatrolBehavior()
    {
        // Implement your patrol logic here
        // For example, moving along a set of waypoints or standing guard
    }

    void ChaseBehavior()
    {
        // Implement your chase logic here
        // For example, moving towards the player's last known position
    }

    void CheckForPlayer1()
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
                    Debug.Log("Cop sees the player spraying!");
                    currentState = CopState.Chase;
                }
            }
        }
        Debug.DrawRay(leftEyePosition, sightDirection, Color.red);
        Debug.DrawRay(rightEyePosition, sightDirection, Color.blue);
    }
}


