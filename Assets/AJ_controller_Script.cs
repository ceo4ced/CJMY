using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AJ_controller_Script : MonoBehaviour
{
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float turnSpeedWalking = 5f; // Rotation speed for the player when walking
    public float turnSpeedIdle = 1f; // Rotation speed for the player when idle turning
    public float turnSpeedRunning = 10f; // Rotation speed for the player when running
    public float cameraRotationDelay = 0.1f; // Delay in camera rotation
    public GameObject SprayCan;
    public GameObject GreenSpray;
    public Transform cameraTransform; // Reference to the main camera's transform

    private CharacterController controller;
    private Animator animator;
    private Rigidbody rb; // Reference to the Rigidbody component
    private Vector3 lastPlayerPosition; // Last position of the player
    private float transitionSpeed = 100f;
    private bool allowInput = true; // Variable to control whether input is allowed or not

    void Start()
    {
        // Disable the spray can GameObject initially
        DeactivateGreenSpray();
        DeactivateSprayCan();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();


        // Initialize lastPlayerPosition
        lastPlayerPosition = transform.position;
    }

    void Update()
    {
        // If input is not allowed (e.g., during spray animation), return without processing input
        if (!allowInput)
            return;

        // Movement input
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Rotate input direction based on player's rotation
        Vector3 inputDirection = RotateInputDirection(horizontalInput, verticalInput);

        // Check if any movement key (W, A, S, D) is pressed
        bool isMoving = inputDirection.magnitude > 0;

        // Movement speed
        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        // Update VelX and VelY parameters based on input
        float velX = 0f;
        float velY = 0f;

        // Determine turn speed based on movement state
        float currentTurnSpeed = 0f;

        if (isMoving)
        {
            // Adjust turn speed while walking or running
            currentTurnSpeed = Input.GetKey(KeyCode.LeftShift) ? turnSpeedRunning : turnSpeedWalking;
        }
        else
        {
            // Adjust turn speed while idle turning
            currentTurnSpeed = turnSpeedIdle;
        }

        // If W key is pressed
        if (verticalInput > 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                // Running forward
                velY = 1f;
            }
            else
            {
                // Walking forward
                velY = 0.3f;
            }
        }
        else if (verticalInput < 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                // Running backward
                velY = -1f;
            }
            else
            {
                // Walking backward
                velY = -0.5f;
            }
            // Rotate the camera to look back when walking backward
            RotateCameraBackward();
        }

        // If W key is pressed with A or D
        if (verticalInput > 0 && Mathf.Abs(horizontalInput) > 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                // Running forward
                velY = 0.60f;
                velX = Mathf.Sign(horizontalInput) * 0.68f; // Set velX to 0.60 or -0.60 for running
            }
            else
            {
                // Walking forward
                walkSpeed = turnSpeedWalking;
                velY = 0.17f;
                velX = Mathf.Sign(horizontalInput) * 0.17f; // Set velX to 0.17 or -0.17 for walking
            }
        }

        // If A or D key is pressed alone
        if (Mathf.Abs(horizontalInput) > 0 && verticalInput == 0)
        {
            // Idle turning
            currentTurnSpeed = turnSpeedIdle;
            velX = Mathf.Sign(horizontalInput) * 0.5f; // Explicit cast to float
            velY = 0.02f;
            // Freeze horizontal movement
            inputDirection = Vector3.zero;

            // Rotate the character in place
            transform.Rotate(transform.up, currentTurnSpeed * horizontalInput * Time.deltaTime);
        }

        // Set animator parameters
        animator.SetFloat("VelX", velX);
        animator.SetFloat("VelY", velY);

        // Rotate player towards movement direction
        if (inputDirection != Vector3.zero && !animator.GetCurrentAnimatorStateInfo(0).IsName("AJ Spray"))
        {
            Quaternion targetRotation = Quaternion.LookRotation(inputDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, currentTurnSpeed * Time.deltaTime);
        }

        // Move the character only when there's input and the spray animation is not playing
        if (isMoving && !animator.GetCurrentAnimatorStateInfo(0).IsName("AJ Spray"))
        {
            // Move the character in the rotated input direction
            controller.Move(inputDirection * speed * Time.deltaTime);

            // Update lastPlayerPosition
            lastPlayerPosition = transform.position;
        }

        // If Q key is pressed and the spray animation is not already playing, set the "Spray" boolean parameter to true
        if (Input.GetKeyDown(KeyCode.Q) && !animator.GetCurrentAnimatorStateInfo(0).IsName("AJ Spray"))
        {
            animator.SetBool("Spray", true);
            // Disable the CharacterController component to freeze character movement during spray animation
            controller.enabled = false;
            // Freeze position in all axes, but allow rotation
            rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
            rb.constraints |= RigidbodyConstraints.FreezeRotation;

            // Disable input
            allowInput = false;
        }

        // Rotate the camera with a delay
        RotateCameraWithDelay();
    }

    Vector3 RotateInputDirection(float horizontalInput, float verticalInput)
    {
        // Get the forward and right vectors based on player's rotation
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        // Project the input direction onto the player's forward and right vectors
        Vector3 inputDirection = forward * verticalInput + right * horizontalInput;

        // Return the rotated input direction
        return inputDirection.normalized;
    }

    void RotateCameraBackward()
    {
        // Rotate the camera to look back
        if (cameraTransform != null)
        {
            cameraTransform.rotation = Quaternion.Euler(0, transform.eulerAngles.y + 180f, 0);
        }
    }

    // method to activate the greenspray gameobject
    void ActivateGreenSpray()
    {
        GreenSpray.SetActive(true);
    }

    // Method to deactivate the GreenSpray GameObject
    void DeactivateGreenSpray()
    {
        GreenSpray.SetActive(false);
    }

    // Method to activate the spray can GameObject
    void ActivateSprayCan()
    {
        SprayCan.SetActive(true);
    }

    // Method to deactivate the spray can GameObject
    void DeactivateSprayCan()
    {
        SprayCan.SetActive(false);
    }

    // Function to reset the "Spray" parameter to false after the spray animation
    public void ResetSprayParameter()
    {
        animator.SetBool("Spray", false);

    }

    public void RegainMotion()
    {
        // Re-enable the CharacterController component after the spray animation is finished
        controller.enabled = true;

        // Unfreeze all position axes (X, Y, Z)
        rb.constraints &= ~RigidbodyConstraints.FreezePositionX;
        rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
        rb.constraints &= ~RigidbodyConstraints.FreezePositionZ;
        rb.constraints |= RigidbodyConstraints.FreezeRotation;

        // Allow input again
        allowInput = true;
    }

    void RotateCameraWithDelay()
    {
        // Rotate the camera to match the player's rotation with a delay
        if (cameraTransform != null)
        {
            cameraTransform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        }
    }
}
