using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AJ_controller_Script : MonoBehaviour
{
    public enum PlayerState
    {
        Normal,
        Spraying
    }

    public PlayerState currentState = PlayerState.Normal;

    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float turnSpeedWalking = 5f; // Rotation speed for the player when walking
    public float turnSpeedIdle = 1f; // Rotation speed for the player when idle turning
    public float turnSpeedRunning = 10f; // Rotation speed for the player when running
    public float cameraRotationDelay = 0.1f; // Delay in camera rotation
    public GameObject SprayCan;
    public GameObject GreenSpray;
    public GameObject Graffiti1;
    public Transform cameraTransform; // Reference to the main camera's transform

    private CharacterController controller;
    private Animator animator;
    private Rigidbody rb; // Reference to the Rigidbody component
    private Vector3 lastPlayerPosition; // Last position of the player
    private bool allowInput = true; // Variable to control whether input is allowed or not

    void Start()
    {
        // Disable the spray can GameObject initially
        DeactivateGraffiti1();
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

        if (Input.GetKeyDown(KeyCode.Q) && !animator.GetCurrentAnimatorStateInfo(0).IsName("AJ Spray"))
        {
            SwitchToSprayingState();
        }
    }

    void HandleSprayingState()
    {
        if (Input.GetKeyDown(KeyCode.Q) && currentState == PlayerState.Spraying)
        {
            SwitchToNormalState();
        }
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
        GreenSpray.SetActive(true);
    }

    void DeactivateGreenSpray()
    {
        GreenSpray.SetActive(false);
    }

    void ActivateSprayCan()
    {
        SprayCan.SetActive(true);
    }

    void DeactivateSprayCan()
    {
        SprayCan.SetActive(false);
    }

    void ActivateGraffiti1()
    {
        Graffiti1.SetActive(true);
    }

    void DeactivateGraffiti1()
    {
        Graffiti1.SetActive(false);
    }

    public void ResetSprayParameter()
    {
        animator.SetBool("Spray", false);
    }

    void RotateCameraWithDelay()
    {
        if (cameraTransform != null)
        {
            cameraTransform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        }
    }
}
