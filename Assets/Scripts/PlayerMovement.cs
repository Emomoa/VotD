using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float sneakSpeed = 2.5f;
    public float acceleration = 10f;
    public float gravity = -9.81f;
    public bool isDead = false;
    public bool isSneaking = false;

    private Vector3 moveDirection = Vector3.zero;
    private Vector3 desiredMoveDirection = Vector3.zero;
    private float velocityY = 0f;
    private bool isGrounded;
    public bool isMoving;

    [Header("Mouse Look Settings")]
    public float mouseSensitivity = 100f;
    private float xRotation = 0f;

    [Header("Footstep Settings")]
    public AudioSource footstepAudioSource;
    public AudioClip[] carpetFootsteps;
    public AudioClip[] woodFootsteps;
    public AudioClip[] tileFootsteps;
    public float footstepInterval = 0.5f; // Minimum time between footsteps
    private float footstepTimer = 0f;
    private bool wasMovingLastFrame = false;

    [Header("References")]
    private CharacterController controller;
    public Transform cameraTransform;

    public static event Action OnPlayerDeath;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        // Lock and hide the cursor
        Cursor.lockState = CursorLockMode.Locked;

        // Initialize rotation
        xRotation = 0f;
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    private void Update()
    {
        HandleMovement();
        //HandleMouseLook();
    }

    private void HandleMovement()
    {
        // Ground check
        isGrounded = controller.isGrounded;

        if (isGrounded && velocityY < 0)
        {
            velocityY = -2f; // Slight negative value to keep grounded
        }

        // Get input
        var speed = Input.GetKey(KeyCode.LeftShift) ? sneakSpeed : walkSpeed;
        isSneaking = Input.GetKey(KeyCode.LeftShift);
        var moveX = Input.GetAxisRaw("Horizontal");
        var moveZ = Input.GetAxisRaw("Vertical");

        // Calculate desired move direction
        desiredMoveDirection = (transform.forward * moveZ + transform.right * moveX).normalized * speed;

        // If there's no input, reset moveDirection to zero
        if (moveX == 0 && moveZ == 0)
        {
            moveDirection = Vector3.zero;
        }
        else
        {
            // Smoothly interpolate moveDirection
            moveDirection = Vector3.Lerp(moveDirection, desiredMoveDirection, acceleration * Time.deltaTime);
        }

        // Apply movement
        var velocity = moveDirection;

        // Apply gravity
        velocityY += gravity * Time.deltaTime;
        velocity.y = velocityY;

        controller.Move(velocity * Time.deltaTime);

        // Reset vertical velocity if grounded
        if (isGrounded && velocityY < 0)
        {
            velocityY = -2f;
        }

        // Handle footstep sounds
        HandleFootsteps();
    }

    private void HandleMouseLook()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Adjust vertical rotation
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Apply rotations
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void HandleFootsteps()
    {
        isMoving = controller.velocity.magnitude > 0.2f && isGrounded;

        if (isMoving)
        {
            footstepTimer -= Time.deltaTime;

            // If the player just started moving, play a footstep sound immediately
            if (!wasMovingLastFrame)
            {
                PlayFootstepSound();
                footstepTimer = footstepInterval; // Reset the timer
            }
            else if (footstepTimer <= 0f)
            {
                PlayFootstepSound();
                footstepTimer = footstepInterval;
            }
        }
        else
        {
            // Reset the footstep timer when the player stops moving
            footstepTimer = footstepInterval;
        }

        wasMovingLastFrame = isMoving;
    }

    private void PlayFootstepSound()
    {
        var groundTag = GetGroundTag();

        if (groundTag == null) return;

        var selectedFootsteps = groundTag switch
        {
            "Wood" => woodFootsteps,
            "Tile" => tileFootsteps,
            "Carpet" => carpetFootsteps,
            _ => carpetFootsteps
        };

        if (selectedFootsteps.Length == 0) return;

        // Play a random footstep sound
        var clipIndex = Random.Range(0, selectedFootsteps.Length);
        footstepAudioSource.pitch = isSneaking ? 0.9f : 1f; 
        footstepAudioSource.PlayOneShot(selectedFootsteps[clipIndex]);
    }

    public string GetGroundTag()
    {
        RaycastHit hit;
        var origin = transform.position + Vector3.up * 0.1f;
        if (Physics.Raycast(origin, Vector3.down, out hit, 2f))
        {
            return hit.collider.tag;
        }
        return null;
    }

    public void Die()
    {
        OnPlayerDeath?.Invoke();
    }
}
