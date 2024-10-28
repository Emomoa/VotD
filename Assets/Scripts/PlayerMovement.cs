using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public ParameterLoader parameterLoader;
    
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
    public AudioClip[] creakyFootsteps;
    public float footstepInterval = 0.5f; // Minimum time between footsteps
    private float footstepTimer = 0f;
    private bool wasMovingLastFrame = false;
    private bool onWeakPlank = false;

    [Header("References")]
    private CharacterController controller;
    public Transform cameraTransform;

    public static event Action OnPlayerDeath;

    private void Awake()
    {
        LoadParameters();
        
        SceneManager.sceneLoaded += OnSceneLoaded;
        controller = GetComponent<CharacterController>();
        
        Cursor.lockState = CursorLockMode.Locked;
        
        xRotation = 0f;
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
    
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadParameters();
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        // Ground check
        isGrounded = controller.isGrounded;

        if (isGrounded && velocityY < 0)
        {
            velocityY = -2f; // Slight negative value to keep grounded
        }

        // Get input and determine speed
        var speed = Input.GetKey(KeyCode.LeftShift) ? sneakSpeed : walkSpeed;
        isSneaking = Input.GetKey(KeyCode.LeftShift);

        // Get movement input only from W, A, S, D keys
        var moveX = 0f;
        var moveZ = 0f;

        if (Input.GetKey(KeyCode.W)) moveZ += 1;
        if (Input.GetKey(KeyCode.S)) moveZ -= 1;
        if (Input.GetKey(KeyCode.A)) moveX -= 1;
        if (Input.GetKey(KeyCode.D)) moveX += 1;

        // Calculate desired move direction
        desiredMoveDirection = (transform.forward * moveZ + transform.right * moveX).normalized * speed;

        
        if (moveX == 0 && moveZ == 0)
        {
            moveDirection = Vector3.zero;
        }
        else
        {
            
            moveDirection = Vector3.Lerp(moveDirection, desiredMoveDirection, acceleration * Time.deltaTime);
        }

        
        var velocity = moveDirection;

        
        velocityY += gravity * Time.deltaTime;
        velocity.y = velocityY;

        controller.Move(velocity * Time.deltaTime);

        
        if (isGrounded && velocityY < 0)
        {
            velocityY = -2f;
        }

        
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
           // "WeakPlank" => creakyFootsteps,
            _ => carpetFootsteps
        };

        if (onWeakPlank)
        {
            selectedFootsteps = creakyFootsteps;
        }

        if (selectedFootsteps.Length == 0) return;

        // Play a random footstep sound
        var clipIndex = Random.Range(0, selectedFootsteps.Length);
        footstepAudioSource.pitch = isSneaking ? 0.9f : 1f; 
        footstepAudioSource.PlayOneShot(selectedFootsteps[clipIndex]);

        AudioClip temp = selectedFootsteps[0];
        selectedFootsteps[0] = selectedFootsteps[clipIndex];
        selectedFootsteps[clipIndex] = temp;
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
    
    public void LoadParameters()
    {
        parameterLoader = FindObjectOfType<ParameterLoader>();
        if (parameterLoader != null && parameterLoader.parameters != null)
        {
            walkSpeed = parameterLoader.parameters.walkSpeed;
            sneakSpeed = parameterLoader.parameters.sneakSpeed;
            acceleration = parameterLoader.parameters.acceleration;
        }
        else
        {
            Debug.LogError("ParameterLoader or parameters not found");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "WeakPlank")
        {
            onWeakPlank = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "WeakPlank")
        {
            onWeakPlank = false;
        }
    }
    

}
