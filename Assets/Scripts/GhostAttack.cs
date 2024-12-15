using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class GhostAttack : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip runningSound;
    public AudioClip initializeAttack;
    public AudioClip KillPlayerSound;
    public AudioClip deflectedSound;

    private GameObject player;
    

    [Header("Variables")]
    [Tooltip("How fast the ghost runs toward the player")]
    [SerializeField] private float speed = 10f;

    [Tooltip("How close the ghost gets to the player")]
    [SerializeField] private float stopDistance = 10f;

    [Tooltip("How far away the ghost teleports around the player")]
    [SerializeField] private float offset = 10;

    [SerializeField] private float attackInterval = 5f;
    public bool canAttack = true;

    private bool shouldRunTowardPlayer = false;
    
    public bool hasReachedPlayer = false;


    // For testing
    private float timer = 0f;

    private PlayerMovement playerMovement;

    private void OnEnable()
    {
        PlayerMovement.OnPlayerDeath += ResetAttack;
    }

    private void OnDisable()
    {
        PlayerMovement.OnPlayerDeath -= ResetAttack;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if(player == null)
        {
            Debug.LogWarning("Could not find gameobject with tag player.");
        }
        playerMovement = player.GetComponent<PlayerMovement>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.E)){
            StartAttack();
            canAttack = false;
        }
        timer += Time.deltaTime;
        /*
        if (canAttack && timer > attackInterval)
        {
            StartAttack();
            canAttack = false;
        }*/


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
                    if(!audioSource.isPlaying)
                    {
                        /*
                        audioSource.clip = runningSound;
                        audioSource.loop = true;
                        audioSource.Play();
                        */
                        audioSource.PlayOneShot(runningSound);
                    }

                }
                else
                {
                    hasReachedPlayer = true;
                    // Has reached the player
                    audioSource.Stop();
                    audioSource.loop = false;
                    shouldRunTowardPlayer = false;

                    // QTE sound
                    audioSource.Stop();
                    //audioSource.PlayOneShot(KillPlayerSound);

                }
            }

        }
    }

    async void StartAttack()
    {
        //Debug.LogWarning("Attack starting");
        //CancelInvoke();

        // Activate collision
        GetComponent<CapsuleCollider>().enabled = true;

        // Randomly choose where to teleport around the player.
        RandomlySelectWhereToTeleport(true);

        await Task.Delay(2500);

        //RandomlySelectWhereToTeleport(true);

        // Run toward player after ? seconds.
        Invoke("RunTowardPlayer", 3f);

    }

    void RunTowardPlayer()
    {
        CancelInvoke();
        //RandomlySelectWhereToTeleport(false);
        shouldRunTowardPlayer = true;

    }

    void TeleportToRight(bool playSound)
    {
        // Move to right of player
        if (player != null)
        {
            //Debug.Log("Teleporting...");
            Vector3 playerPosition = player.transform.position;
            Vector3 newPosition = playerPosition + player.transform.right * offset;
            transform.position = newPosition;

            if(playSound)
            {
                // Handle sound
                PlayShortRunSound();
            }
        }
        else
        {
            Debug.LogWarning("Player is null");
        }
    }
    void TeleportToLeft(bool playSound)
    {
        // Move to left of player
        if (player != null)
        {
            //Debug.Log("Teleporting...");
            Vector3 playerPosition = player.transform.position;
            Vector3 newPosition = playerPosition - player.transform.right * offset;
            transform.position = newPosition;

            if (playSound)
            {
                // Handle sound
                PlayShortRunSound();
            }

        }
        else
        {
            Debug.LogWarning("Player is null");
        }


    }

    void TeleportToFront(bool playSound)
    {
        if (player != null)
        {
            Vector3 playerPosition = player.transform.position;
            Vector3 newPosition = playerPosition + player.transform.forward * offset;
            transform.position = newPosition;

            if (playSound)
            {
                // Handle sound
                PlayShortRunSound();
            }

        }
        else
        {
            Debug.LogWarning("Player is null");
        }
    }

    void PlayShortRunSound()
    {
        audioSource.clip = initializeAttack;
        audioSource.loop = false;
        audioSource.Play();
    }
    
    public void ResetAttack()
    {
        canAttack = true;
        timer = 0f;
        attackInterval = Random.Range(20, 31);
    }

    // Helpmethod
    void RandomlySelectWhereToTeleport(bool playSound)
    {
        //CancelInvoke();
        int randomChoice = Random.Range(0, 3);
        switch (randomChoice)
        {
            case 0:
                TeleportToRight(playSound);
                break;
            case 1:
                TeleportToLeft(playSound);
                break;
            case 2:
                TeleportToFront(playSound);
                break;
        }
    }
}
