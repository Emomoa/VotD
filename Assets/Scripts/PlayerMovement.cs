using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public bool IsSneaking = false;
    public float rayDistance = 10f; // Distance for forward raycast

    [SerializeField]
    private CharacterController controller;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip[] carpetFootsteps;
    [SerializeField]
    private AudioClip[] woodFootsteps;
    [SerializeField]
    private AudioClip[] tileFootsteps;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Handle sneaking
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Sneak();
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            StopSneaking();
        }

        // Get input from keyboard
        float moveX = Input.GetAxis("Horizontal"); // Left and right
        float moveZ = Input.GetAxis("Vertical");   // Forward and backward

        // Create a movement vector in world space
        Vector3 move = new Vector3(moveX, 0, moveZ);

        // If there is movement input, update player's rotation and position
        if (move != Vector3.zero)
        {
            // Move player
            controller.Move(move.normalized * speed * Time.deltaTime); // Normalize movement vector for consistent speed
            PlayFoley();
        }

        // Shoot a raycast from the player in the look direction
        RaycastHit hit;
        Vector3 rayDirection = transform.forward; // Shoot the ray forward
        if (Physics.Raycast(transform.position, rayDirection, out hit, rayDistance))
        {
            Debug.Log("Looking at: " + hit.collider.name); // Debug to see what we hit
        }

        // Draw a raycast to show the direction
        Debug.DrawRay(transform.position, rayDirection * rayDistance, Color.red);
    }

    void Sneak()
    {
        IsSneaking = true;
        speed = 2.5f;
    }

    void StopSneaking()
    {
        IsSneaking = false;
        speed = 5f; // Reset speed to normal
    }

    void PlayFoley()
    {
        if (!audioSource.isPlaying)
        {
            // Raycast downward to check the ground surface
            RaycastHit groundHit;
            Vector3 rayOrigin = transform.position; // Start ray a little above the player to avoid collisions with itself
            if (Physics.Raycast(rayOrigin, Vector3.down, out groundHit, 2f))
            {
                string groundTag = groundHit.collider.tag;
                Debug.Log("Ground hit " + groundTag);

                // Select the appropriate footstep sound based on the tag of the ground
                AudioClip[] selectedFootsteps = carpetFootsteps; // Default footsteps

                if (groundTag == "Wood")
                {
                    selectedFootsteps = woodFootsteps;
                }
                else if (groundTag == "Tile")
                {
                    selectedFootsteps = tileFootsteps;
                }
                else if (groundTag == "Carpet")
                {
                    selectedFootsteps = carpetFootsteps;
                }

                // Play the selected footstep sound
                int clip = Random.Range(0, selectedFootsteps.Length);
                audioSource.clip = selectedFootsteps[clip];

                // Adjust pitch for sneaking
                audioSource.pitch = IsSneaking ? 0.5f : 1f;
                audioSource.Play();
            }
        }
    }
}
