using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip quickTimeEventQue;
    public AudioClip swingTorchSound;
    public AudioSource audioSource;
    public AudioClip DeathSound;
    public AudioClip MissedAttackSound;
    public AudioClip ghostGone;

    [Header("Variables")]
    [SerializeField] private float TimeToDeflect = 2f;
    private float deflectWindowTimer = 0f;
    public PlayerMovement playerMovement;

    [Header("Raycasting")]
    public float boxHalfExtent = 0.5f;  // Half the size of the box in each dimension
    public float rayDistance = 10f;
    public LayerMask layerMask;

    private Torch torch;
    private GhostAttack ghost;

    private bool isDeflecting = false;
    private bool isLookingAtGhost = false;
    private bool canAttack = true;

    [SerializeField] private float cooldownDuration = 2f;
    private float cooldownTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        torch = GetComponent<Torch>();
        ghost = FindObjectOfType<GhostAttack>();
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();

        // check if ghost should activate QTE.
        if (ghost != null)
        {
            HandleQTE();
        }
    }

    void Attack()
    {
        canAttack = false;
        cooldownTime = cooldownDuration;
        audioSource.PlayOneShot(swingTorchSound);
    }
    async void HandleRaycast(Vector3 rayDirection)
    {
        // Perform the raycast
        if (Physics.BoxCast(transform.position, new Vector3(boxHalfExtent, boxHalfExtent, boxHalfExtent),
                                rayDirection, out RaycastHit hit, transform.rotation, rayDistance, layerMask))
        {
            isLookingAtGhost = true;
            Debug.Log("Box hit object: " + hit.collider.name);
        }
        else
        {
            // Log if the raycast hit nothing
            Debug.Log("No hit detected.");
            await Task.Delay(250);
            audioSource.PlayOneShot(MissedAttackSound);
        }
        Debug.DrawRay(transform.position, rayDirection * rayDistance, Color.green, 1f);
    }

    void HandleInput()
    {
        // Handle timer stuff
        if (cooldownTime > 0)
        {
            cooldownTime -= Time.deltaTime;
            //Debug.Log(cooldownTime);
        }

        if(cooldownTime <= 0f)
        {
            canAttack = true;
        }

        // Can player attack?
        if (canAttack)
        {
            // Hit infront of player
            if (Input.GetKey(KeyCode.Keypad8))
            {
                // spela ljud samt aktivera cooldown.
                Attack();
                // Raycast to the left of player
                HandleRaycast(transform.forward);
               
            }

            // Hit to the left of player
            if (Input.GetKey(KeyCode.Keypad4))
            {
                // spela ljud samt aktivera cooldown.
                Attack();
                HandleRaycast(-transform.right);

            }

            // Hit to the right of player
            if (Input.GetKey(KeyCode.Keypad6))
            {
                // spela ljud samt aktivera cooldown.
                Attack();
                HandleRaycast(transform.right);

            }

        }
        
    }
    async void HandleQTE()
    {
        if (ghost.hasReachedPlayer)
        {
            isDeflecting = true;
            audioSource.PlayOneShot(quickTimeEventQue);
            deflectWindowTimer = TimeToDeflect;
            ghost.hasReachedPlayer = false;
        }

        if (isDeflecting)
        {
            deflectWindowTimer -= Time.deltaTime;

            // Player deflected
            if (torch.GetIsLit() && isLookingAtGhost && deflectWindowTimer > 0)
            {
                Debug.Log("Deflected ghost!");
                Debug.Log(deflectWindowTimer);

                // Play ghost scream sound
                ghost.audioSource.Stop();
                audioSource.PlayOneShot(ghost.deflectedSound);
                EndDeflectWindow();
                ghost.ResetAttack();
                await Task.Delay(1000);
                audioSource.PlayOneShot(ghostGone);


            }
            if (deflectWindowTimer <= 0)
            {
                //EndDeflectWindow();
                isDeflecting = false;
                audioSource.PlayOneShot(ghost.KillPlayerSound);
                // Wait before playing death sound
                await Task.Delay(1000);
                audioSource.PlayOneShot(DeathSound);
                playerMovement.Die();
            }
        }
    }

    void EndDeflectWindow()
    {
        isDeflecting = false;               // End the deflect window
        deflectWindowTimer = TimeToDeflect;            // Reset the timer
        
        ghost.GetComponent<CapsuleCollider>().enabled = false;
        ghost.ResetAttack();
        isLookingAtGhost = false;
        
        
    }

}
