using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GhostAttack : MonoBehaviour
{
    public AudioClip runningShort;
    public AudioClip runningLong;
    public AudioClip quickTimeEventQue;
    public AudioClip deflectedSound;


    [Tooltip("How fast the ghost runs toward the player")]
    [SerializeField]
    private float speed = 10f;

    [Tooltip("How close the ghost gets to the player")]
    [SerializeField]
    private float stopDistance = 10f;

    [Tooltip("How far away the ghost teleports around the player")]
    [SerializeField]
    private float offset = 10;

    [SerializeField]
    private float deflectWindowTime = 10f;
    private float deflectWindowTimer = 0f;

    private GameObject player;
    private AudioSource audioSource;

    private bool shouldAttack = false;
    private bool shouldRunTowardPlayer = false;
    private bool isDeflecting = false;
    private bool shouldQTE = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if(player == null)
        {
            Debug.LogWarning("Could not find gameobject with tag player.");
        }

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // Debug
        if (Input.GetKeyDown(KeyCode.T))
        {
            shouldAttack = true;
        }

        // Attacks player
        if (shouldAttack)
        {
            StartAttack();
            shouldAttack = false;
        }

        // Runs toward player
        if (shouldRunTowardPlayer)
        {
            if (player != null)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

                // Only move towards the player if the object is farther than the stop distance
                if (distanceToPlayer > stopDistance)
                {
                    // Move our position a step closer to the target.
                    var step = speed * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
                }
                else
                {
                    // Has reached the player
                    audioSource.Stop();
                    audioSource.loop = false;
                    shouldRunTowardPlayer = false;

                    // QTE sound
                    audioSource.clip = quickTimeEventQue;
                    audioSource.loop = false;
                    audioSource.Play();

                    // Handle QTE
                    shouldQTE = true;

                }
            }

        }
        if (shouldQTE)
        {
            isDeflecting = true;
            HandleQTE();
        }
    }

    void StartAttack()
    {
        // Randomly choose where to teleport around the player.
        int randomChoice = Random.Range(0, 4);
        switch (randomChoice)
        {
            case 0:
                TeleportToRight();
                break;
            case 1:
                TeleportToLeft();
                break;
            case 2:
                TeleportToFront();
                break;
            case 3:
                TeleportToBehind();
                break;
            default:
                TeleportToRight();
                break;
        }

        CancelInvoke();
        // Run toward player after ? seconds.
        Invoke("RunTowardPlayer", 2f);

    }

    void RunTowardPlayer()
    {
        CancelInvoke();
        PlayLongRunSound();
        shouldRunTowardPlayer = true;

    }

    void TeleportToRight()
    {
        // Move to right of player
        if (player != null)
        {
            //Debug.Log("Teleporting...");
            Vector3 playerPosition = player.transform.position;
            Vector3 newPosition = playerPosition + player.transform.right * offset;
            transform.position = newPosition;

            // Handle sound
            PlayShortRunSound();

        }
        else
        {
            Debug.LogWarning("Player is null");
        }
    }
    void TeleportToLeft()
    {
        // Move to left of player
        if (player != null)
        {
            //Debug.Log("Teleporting...");
            Vector3 playerPosition = player.transform.position;
            Vector3 newPosition = playerPosition - player.transform.right * offset;
            transform.position = newPosition;

            // Handle sound
            PlayShortRunSound();

        }
        else
        {
            Debug.LogWarning("Player is null");
        }


    }

    void TeleportToFront()
    {
        if (player != null)
        {
            Vector3 playerPosition = player.transform.position;
            Vector3 newPosition = playerPosition + player.transform.forward * offset;
            transform.position = newPosition;

            // Handle sound
            PlayShortRunSound();

        }
        else
        {
            Debug.LogWarning("Player is null");
        }
    }

    void TeleportToBehind()
    {
        if (player != null)
        {
            Vector3 playerPosition = player.transform.position;
            Vector3 newPosition = playerPosition - player.transform.forward * offset;
            transform.position = newPosition;

            // Handle sound
            PlayShortRunSound();
        }
        else
        {
            Debug.LogWarning("Player is null");
        }
    }

    void PlayShortRunSound()
    {
        audioSource.clip = runningShort;
        audioSource.loop = false;
        audioSource.Play();
    }

    void PlayLongRunSound()
    {
        audioSource.clip = runningLong;
        audioSource.loop = true;
        audioSource.Play();
    }
    
    void HandleQTE()
    {
        if (isDeflecting)
        {
            deflectWindowTimer += Time.deltaTime;

            Debug.LogWarning("WINDOW TIMER: " + deflectWindowTimer);

            // Test deflect
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Deflected ghost!");
                audioSource.clip = deflectedSound;
                EndDeflectWindow();


            }
            else if (deflectWindowTimer >= deflectWindowTime)
            {
                // Time is up, end the deflect window
                Debug.Log("Deflect window expired!");
                EndDeflectWindow();
            }
        }
    }

    void EndDeflectWindow()
    {
        isDeflecting = false;       // End the deflect window
        deflectWindowTimer = 0f;    // Reset the timer
        shouldQTE = false;          // End QTE.
    }
}
