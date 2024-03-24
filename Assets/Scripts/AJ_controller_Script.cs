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
    public GameObject GreenCan;
    public GameObject BlueCan;
    public GameObject RedCan;
    public GameObject GreenSpray;
    public GameObject BlueSpray;
    public GameObject RedSpray;
    public GameObject Graffiti1;
    public Transform AllGraffiti;
    public Transform cameraTransform; // Reference to the main camera's transform

    private CharacterController controller;
    private Animator animator;
    private Rigidbody rb; // Reference to the Rigidbody component
    private Vector3 lastPlayerPosition; // Last position of the player
    private bool allowInput = true; // Variable to control whether input is allowed or not
    private bool hasRedCan;
    private bool hasBlueCan;
    private bool hasGreenCan;

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
    }


    void ActivateGraffiti1()
    {
        GameObject graffitiToColor = retrieveClosestValidGraffiti();
        if (hasBlueCan)
        {
            //do blue things
            SpriteRenderer spriteRenderer = graffitiToColor.GetComponent<SpriteRenderer>();
            spriteRenderer.color = Color.blue;
            graffitiToColor.SetActive(true);
        }
        if (hasRedCan)
        {
            // do red things
            SpriteRenderer spriteRenderer = graffitiToColor.GetComponent<SpriteRenderer>();
            spriteRenderer.color = Color.red;
            graffitiToColor.SetActive(true);
        }
        if (hasGreenCan)
        {
            // do green things
            SpriteRenderer spriteRenderer = graffitiToColor.GetComponent<SpriteRenderer>();
            spriteRenderer.color = Color.green;
            graffitiToColor.SetActive(true);
        }
        hasBlueCan = false;
        hasRedCan = false;
        hasGreenCan = false;
    }

    void DeactivateGraffiti1()
    {
        Graffiti1.SetActive(false);
    }

    void DeactivateAllGraffiti()
    {
        print(AllGraffiti.childCount);
        foreach (Transform graffiti in AllGraffiti) {
            graffiti.gameObject.SetActive(false);
        }
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
