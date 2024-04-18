using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AJ_controller_Script : MonoBehaviour
{
    public enum PlayerState
    {
        Normal,
        Spraying,
        Caught
    }

    public PlayerState currentState = PlayerState.Normal;

    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float turnSpeedWalking = 5f; // Rotation speed for the player when walking
    public float turnSpeedIdle = 1f; // Rotation speed for the player when idle turning
    public float turnSpeedRunning = 10f; // Rotation speed for the player when running
    public float cameraRotationDelay = 0.1f; // Delay in camera rotation
    public GameObject GreenCan;
    public GameObject BlueCan;
    public GameObject RedCan;
    public GameObject GreenSpray;
    public GameObject BlueSpray;
    public GameObject RedSpray;
    public GameObject Graffiti1;
    public Transform AllGraffiti;
    public Transform cameraTransform; // Reference to the main camera's transform
    public GameObject TutorialText;
    public GameObject ObjectiveText;
    public TMPro.TextMeshProUGUI scoreText;
    public AudioClip spraySound;

    private CharacterController controller;
    private Animator animator;
    private Rigidbody rb; // Reference to the Rigidbody component
    private Vector3 lastPlayerPosition; // Last position of the player
    private bool allowInput = true; // Variable to control whether input is allowed or not
    private bool hasRedCan;
    private bool hasBlueCan;
    private bool hasGreenCan;
    private int score = 0; // Initialize the score

    void Start()
    {
        // Disable the spray can GameObject initially
        DeactivateGraffiti1();

        // Disable all tutorial Graffiti
        DeactivateAllGraffiti();
        DeactivateGreenSpray();
        DeactivateSprayCan();
        
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        // Initialize lastPlayerPosition
        lastPlayerPosition = transform.position;
        Debug.Log("Initial player state: " + currentState.ToString());
    }

    void Update()
    {
        switch (currentState)
        {
            case PlayerState.Normal:
                HandleNormalState();
                break;
            case PlayerState.Spraying:
                HandleSprayingState();
                break;
            case PlayerState.Caught: // Handle the Caught state
                HandleCaughtState();
                break;
            default:
                break;
        }

        RotateCameraWithDelay();
    }

    void HandleNormalState()
    {
        if (!allowInput)
            return;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 inputDirection = RotateInputDirection(horizontalInput, verticalInput);
        bool isMoving = inputDirection.magnitude > 0;
        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        float velX = 0f;
        float velY = 0f;
        float currentTurnSpeed = 0f;

        if (isMoving)
        {
            currentTurnSpeed = Input.GetKey(KeyCode.LeftShift) ? turnSpeedRunning : turnSpeedWalking;
        }
        else
        {
            currentTurnSpeed = turnSpeedIdle;
        }

        if (verticalInput > 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                velY = 1f;
            }
            else
            {
                velY = 0.3f;
            }
        }
        else if (verticalInput < 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                velY = -1f;
            }
            else
            {
                velY = -0.5f;
            }
            RotateCameraBackward();
        }

        if (verticalInput > 0 && Mathf.Abs(horizontalInput) > 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                velY = 0.60f;
                velX = Mathf.Sign(horizontalInput) * 0.68f;
            }
            else
            {
                walkSpeed = turnSpeedWalking;
                velY = 0.17f;
                velX = Mathf.Sign(horizontalInput) * 0.17f;
            }
        }

        if (Mathf.Abs(horizontalInput) > 0 && verticalInput == 0)
        {
            currentTurnSpeed = turnSpeedIdle;
            velX = Mathf.Sign(horizontalInput) * 0.5f;
            velY = 0.02f;
            inputDirection = Vector3.zero;

            transform.Rotate(transform.up, currentTurnSpeed * horizontalInput * Time.deltaTime);
        }

        animator.SetFloat("VelX", velX);
        animator.SetFloat("VelY", velY);

        if (inputDirection != Vector3.zero && !animator.GetCurrentAnimatorStateInfo(0).IsName("AJ Spray"))
        {
            Quaternion targetRotation = Quaternion.LookRotation(inputDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, currentTurnSpeed * Time.deltaTime);
        }

        if (isMoving && !animator.GetCurrentAnimatorStateInfo(0).IsName("AJ Spray"))
        {
            controller.Move(inputDirection * speed * Time.deltaTime);
            lastPlayerPosition = transform.position;
        }
      
        if (Input.GetKeyDown(KeyCode.Q) && !animator.GetCurrentAnimatorStateInfo(0).IsName("AJ Spray") && (hasRedCan || hasGreenCan || hasBlueCan))
        {
            SwitchToSprayingState();
        }
    }

    void HandleSprayingState()
    {
        if (Input.GetKeyDown(KeyCode.Q) && currentState == PlayerState.Spraying)
        {
            SwitchToNormalState();
            DeactivateSprayCan(); //Martin Edit
            DeactivateGreenSpray();//Martin Edit
            ResetSprayParameter();//Martin Edit
        }
    }

    void HandleCaughtState()
    {
        // Disable player movement
        allowInput = false;
        animator.SetFloat("VelX", 0f);
        animator.SetFloat("VelY", 0f);
        rb.constraints = RigidbodyConstraints.FreezeAll; // Freeze the player

    }

    void SwitchToSprayingState()
    {
        currentState = PlayerState.Spraying;
        rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
        rb.constraints |= RigidbodyConstraints.FreezeRotation;
        animator.SetBool("Spray", true);
        controller.enabled = false;
        allowInput = false;
    }

    void SwitchToNormalState()
    {
        currentState = PlayerState.Normal;
        controller.enabled = true;
        rb.constraints &= ~RigidbodyConstraints.FreezePositionX;
        rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
        rb.constraints &= ~RigidbodyConstraints.FreezePositionZ;
        rb.constraints |= RigidbodyConstraints.FreezeRotation;
        allowInput = true; // Add this line to enable input control
        Debug.Log("Character switched back to normal state.");
    }

    Vector3 RotateInputDirection(float horizontalInput, float verticalInput)
    {
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;
        Vector3 inputDirection = forward * verticalInput + right * horizontalInput;
        return inputDirection.normalized;
    }

    void RotateCameraBackward()
    {
        if (cameraTransform != null)
        {
            cameraTransform.rotation = Quaternion.Euler(0, transform.eulerAngles.y + 180f, 0);
        }
    }

    void ActivateGreenSpray()
    {
        AudioSource audioSource = GetComponent<AudioSource>();

        // Ensure the AudioSource is set up for looping
        if (audioSource != null && spraySound != null)
        {
            audioSource.clip = spraySound;
            audioSource.loop = true; // Make sure the sound loops
            audioSource.Play();
        }

        if (hasGreenCan)
        {
            GreenSpray.SetActive(true);
        }
        if (hasBlueCan)
        {
            BlueSpray.SetActive(true);
        }
        if (hasRedCan)
        {
            RedSpray.SetActive(true);
        }
    }


    void DeactivateGreenSpray()
    {
        AudioSource audioSource = GetComponent<AudioSource>();

        // Stop the spray sound if it is playing
        if (audioSource != null)
        {
            audioSource.Stop();
            audioSource.loop = false; // Optionally turn off looping, though it stops anyway
        }

        GreenSpray.SetActive(false);
        RedSpray.SetActive(false);
        BlueSpray.SetActive(false);
        EmptyItemInBag();
    }


    void ActivateSprayCan()
    {
        if (hasGreenCan)
        {
            GreenCan.SetActive(true);
        }
        else if (hasRedCan)
        {
            RedCan.SetActive(true);
        }
        else if (hasBlueCan)
        {
            BlueCan.SetActive(true);
        }
    }

    void DeactivateSprayCan()
    {
        GreenCan.SetActive(false);
        BlueCan.SetActive(false);
        RedCan.SetActive(false);
        hasBlueCan = false;
        hasRedCan = false;
        hasGreenCan = false;
        EmptyItemInBag();
    }


    void ActivateGraffiti1()
    {
        GameObject graffitiToColor = retrieveClosestValidGraffiti();
        if (graffitiToColor == null) return; // If no valid graffiti is close enough, exit the method.

        // Check if the graffiti has already been painted.
        GraffitiPaintedTracker graffitiTracker = graffitiToColor.GetComponent<GraffitiPaintedTracker>();
        SpriteRenderer spriteRenderer = graffitiToColor.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) return; // Safety check for SpriteRenderer component.

        if (graffitiTracker != null && graffitiTracker.hasBeenPainted)
        {
            // Already painted graffiti logic: repaint but no score change.
            if (hasGreenCan)
            {
                spriteRenderer.color = Color.green; // Reapply green color
            }
            else if (hasBlueCan)
            {
                spriteRenderer.color = Color.blue; // Reapply blue color
            }
            else if (hasRedCan)
            {
                spriteRenderer.color = Color.red; // Reapply red color
            }

            graffitiToColor.SetActive(true); // Ensure the graffiti is visible.
                                             // No score update and return to avoid changing any other game state.
            return;
        }

        // Paint the graffiti and mark it as painted.
        if (hasGreenCan)
        {
            spriteRenderer.color = Color.green;
            score += 10; // Increment score for green
        }
        else if (hasBlueCan)
        {
            spriteRenderer.color = Color.blue;
            score += 20; // Increment score for blue
        }
        else if (hasRedCan)
        {
            spriteRenderer.color = Color.red;
            score += 50; // Increment score for red
        }

        graffitiToColor.SetActive(true);
        if (graffitiTracker != null)
        {
            graffitiTracker.hasBeenPainted = true; // Mark the graffiti as painted.
        }

        // Update the score display and other UI elements
        scoreText.text = "Score: " + score;
        TutorialText.GetComponent<TMPro.TextMeshPro>().text = "Great job!\nNow go and complete your first objective.";
        ObjectiveText.GetComponent<TMPro.TextMeshProUGUI>().text = "Objective:\nHead to the main square";
    }

    void DeactivateGraffiti1()
    {
        Graffiti1.SetActive(false);
    }
    void DeactivateAllGraffiti()
    {
        foreach (Transform graffiti in AllGraffiti) {
            graffiti.gameObject.SetActive(false);
        }
    }

    public void TookDamage(){
        //switch back to normal state when taking damage so we can run.

        // Choose a random deduction value: 10, 20, or 30.
        int[] deductionOptions = { 10, 20, 30 };
        int deduction = deductionOptions[Random.Range(0, deductionOptions.Length)];

        // Deduct the randomly chosen amount from the score.
        score -= deduction;

        // Ensure score doesn't drop below zero.
        if (score < 0) score = 0;

        // Update UI if necessary
        scoreText.text = "Score: " + score;

        SwitchToNormalState();
        DeactivateSprayCan();
        DeactivateGreenSpray();
        ResetSprayParameter();
    }

    /**
     * Retrieve the graffiti GameObject that is closest within a certain range.
     */
    GameObject retrieveClosestValidGraffiti()
    {
        GameObject closestValidGraffiti = null;
        float shortestDistance = float.MaxValue;
        foreach (Transform graffiti in AllGraffiti)
        {
            float distance = Vector3.Distance(graffiti.position, this.transform.position);
            if(distance < 10.0f && distance < shortestDistance)
            {
                shortestDistance = distance;
                closestValidGraffiti = graffiti.gameObject;
            }
        }
        return closestValidGraffiti;
    }
    public void ResetSprayParameter()
    {
        animator.SetBool("Spray", false);
    }

    public void SetColorItem(string itemTag)
    {
        switch (itemTag)
        {
            case "redCan":
                hasRedCan = true;
                break;
            case "blueCan":
                hasBlueCan = true;
                break;
            case "greenCan":
                hasGreenCan = true;
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the other object has the tag "cop"
        if (other.CompareTag("Cop"))
        {
            // Get the PoliceAIWaypoint script from the cop
            PoliceAIWaypoint copAI = other.GetComponent<PoliceAIWaypoint>();

            // Check if the cop is in Chase, Attack, or Arrest state
            if (copAI != null && (copAI.GetCurrentState() == PoliceAIWaypoint.CopState.Chase ||
                                  copAI.GetCurrentState() == PoliceAIWaypoint.CopState.Attack ||
                                  copAI.GetCurrentState() == PoliceAIWaypoint.CopState.Arrest))
            {
                // Set the IsCaught parameter in the Animator
                animator.SetBool("IsCaught", true);

                // Add whatever logic you use to deactivate green spray or handle the caught state
                DeactivateGreenSpray();
           
                // Assuming you have a currentState variable to set
                currentState = PlayerState.Caught;
            
            }
        }
    }

    public PlayerState GetCurrentState()
    {
        return currentState;
    }

    public void EmptyItemInBag()
    {
        itemCollector collector = GetComponent<itemCollector>();
        if (collector != null)
        {
            collector.ResetItem(); // Reset the item in the item collector
        }
    }

    void RotateCameraWithDelay()
    {
        if (cameraTransform != null)
        {
            cameraTransform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        }
    }
}
