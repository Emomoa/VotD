using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float sneakSpeed = 2.5f;
    private float currentSpeed;
    public float gravity = -9.81f;
    public float acceleration = 10f;

    public bool isSneaking;

    [Header("Physics Settings")]
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 velocity;
    private bool isGrounded;

    [Header("Raycast Settings")]
    public float rayDistance = 10f; // Avstånd för framåtriktad raycast

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip[] carpetFootsteps;
    public AudioClip[] woodFootsteps;
    public AudioClip[] tileFootsteps;
    public float footstepDelay = 0.5f; // Fördröjning mellan fotstegsljud
    private float footstepTimer = 0f;

    [Header("Torch Settings")]
    public Light torchLight; // Tilldela denna i Inspektorn

    [Header("References")]
    private CharacterController controller;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        HandleMovement();
        HandleRaycasting();
        HandleTorch();
    }

    private void HandleMovement()
    {
        // Kontrollera om spelaren är på marken
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -1f; // Håll spelaren på marken
        }

        // Hämta inmatning från tangentbordet
        float targetSpeed = Input.GetKey(KeyCode.LeftShift) ? sneakSpeed : walkSpeed;
        isSneaking = Input.GetKey(KeyCode.LeftShift);
        float moveX = Input.GetAxisRaw("Horizontal"); // Omedelbar inmatning för responsivitet
        float moveZ = Input.GetAxisRaw("Vertical");

        // Beräkna önskad rörelseriktning i lokalt rumskoordinatsystem
        Vector3 desiredMove = transform.right * moveX + transform.forward * moveZ;
        desiredMove = desiredMove.normalized * targetSpeed;

        // Smidigt accelerera mot önskad hastighet
        moveDirection = Vector3.Lerp(moveDirection, desiredMove, acceleration * Time.deltaTime);

        // Applicera rörelse
        controller.Move(moveDirection * Time.deltaTime);

        // Applicera gravitation
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Spela fotstegsljud
        if (isGrounded && moveDirection.magnitude > 0.1f)
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0f)
            {
                PlayFootsteps();
                footstepTimer = footstepDelay / (targetSpeed / walkSpeed); // Justera fördröjning baserat på hastighet
            }
        }
        else
        {
            footstepTimer = 0f; // Återställ timer när spelaren inte rör sig
        }
    }

    private void HandleRaycasting()
    {
        // Skjut en raycast från spelaren i blickriktningen
        RaycastHit hit;
        Vector3 rayDirection = transform.forward;
        if (Physics.Raycast(transform.position, rayDirection, out hit, rayDistance))
        {
            Debug.Log("Tittar på: " + hit.collider.name);
        }

        // Visa raycasten för debugging
        Debug.DrawRay(transform.position, rayDirection * rayDistance, Color.red);
    }

    public string GetGroundTag()
    {
        // Raycast nedåt för att kontrollera underlaget
        RaycastHit groundHit;
        Vector3 rayOrigin = transform.position + Vector3.up * 0.1f; // Starta rayen strax ovanför spelarens fötter

        // Skapa en LayerMask som exkluderar "Player"-lagret
        int layerMask = ~LayerMask.GetMask("Player"); // Invertera masken för att exkludera "Player"-lagret

        if (Physics.Raycast(rayOrigin, Vector3.down, out groundHit, 2f, layerMask))
        {
            return groundHit.collider.tag;
        }
        else
        {
            Debug.LogWarning("Inget underlag detekterades under spelaren!");
            return null;
        }
    }

    private void HandleTorch()
    {
        string groundTag = GetGroundTag();

        if (groundTag == "Carpet")
        {
            if (!torchLight.enabled)
            {
                torchLight.enabled = true;
                Debug.Log("Fackla aktiverad");
            }
        }
        else
        {
            if (torchLight.enabled)
            {
                torchLight.enabled = false;
                Debug.Log("Fackla deaktiverad");
            }
        }
    }

    private void PlayFootsteps()
    {
        if (!audioSource.isPlaying)
        {
            string groundTag = GetGroundTag();

            if (groundTag != null)
            {
                Debug.Log("Underlag: " + groundTag);

                // Välj lämpliga fotstegsljud baserat på underlagets tagg
                AudioClip[] selectedFootsteps = carpetFootsteps; // Standardfotsteg

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

                // Spela det valda fotstegsljudet
                int clipIndex = Random.Range(0, selectedFootsteps.Length);
                audioSource.clip = selectedFootsteps[clipIndex];

                // Justera tonhöjd för smygläge
                audioSource.pitch = isSneaking ? 0.9f : 1f;
                audioSource.Play();
            }
        }
    }

    public void Die()
    {
        // Hantera spelarens död och återupplivning
        Debug.Log("Spelaren har dött.");
        // Implementera återupplivningslogik här
    }
}
