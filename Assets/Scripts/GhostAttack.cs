using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class GhostAttack : MonoBehaviour
{
    public ParameterLoader parameterLoader;
    
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip runningSound;
    public AudioClip quickTimeEventQue;
    public AudioClip deflectedSound;
    public AudioClip swingTorchSound;

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

    public bool isRightOfPlayer = false;
    public bool isLeftOfPlayer = false;
    public bool isFrontOfPlayer = false;
    public bool isBackOfPlayer = false;


    private bool shouldQTE = false;
    private bool shouldRunTowardPlayer = false;

    private bool gotDeflected;
    
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

    private void Awake()
    {
        parameterLoader = FindObjectOfType<ParameterLoader>();
        if (parameterLoader != null && parameterLoader.parameters != null)
        {
            speed = parameterLoader.parameters.ghostSpeed;
            attackInterval = parameterLoader.parameters.attackInterval;
        }
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
        // Handles timer for when the ghost should attack.
        // Ghost attack every 10 seconds for example.
        timer += Time.deltaTime;
        if (canAttack && timer > attackInterval)
        {
            StartAttack();
            canAttack = false;
        }

        // Handles logic for the ghost to run toward player
        if(shouldRunTowardPlayer){
            RunTowardPlayer();
        }

        HandleDeflected();
        
    }

    void HandleDeflected(){
        if(gotDeflected){
            audioSource.Stop();
            audioSource.PlayOneShot(deflectedSound);
            gotDeflected = false;
        }
    }
    void RunTowardPlayer(){
            if (player != null)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

                // Only move towards the player if the object is farther than the stop distance
                if (distanceToPlayer > stopDistance)
                {
                    // Move our position a step closer to the target.
                    var step = speed * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
                    if (!audioSource.isPlaying)
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
                    // Has reached the player
                    audioSource.Stop();
                    audioSource.loop = false;
                    shouldRunTowardPlayer = false;

                    // QTE sound
                    audioSource.Stop();
                    audioSource.PlayOneShot(quickTimeEventQue);

                    // Handle QTE
                    shouldQTE = true;
                    

                }
        }
    }
    async void StartAttack()
    {
        // Activate collision
        GetComponent<CapsuleCollider>().enabled = true;

        // Randomly choose where to teleport around the player.
        RandomlySelectWhereToTeleport(true);

        // Wait X seconds then start running toward player.
        await Task.Delay(3000);
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
            isRightOfPlayer = true;
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
            isLeftOfPlayer = true;
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
            isFrontOfPlayer = true;
        }
        else
        {
            Debug.LogWarning("Player is null");
        }
    }

    void TeleportToBehind(bool playSound)
    {
        if (player != null)
        {
            Vector3 playerPosition = player.transform.position;
            Vector3 newPosition = playerPosition - player.transform.forward * offset;
            transform.position = newPosition;
            isBackOfPlayer = true;
        }
        else
        {
            Debug.LogWarning("Player is null");
        }
    }
    
    public void ResetAttack()
    {
        canAttack = true;
        timer = 0f;
        attackInterval = Random.Range(20, 31);
        // Reset where the ghost is compared to the player.
        isFrontOfPlayer = false;
        isBackOfPlayer = false;
        isLeftOfPlayer = false;
        isRightOfPlayer = false;
    }

    void RandomlySelectWhereToTeleport(bool playSound)
    {
        //CancelInvoke();
        int randomChoice = Random.Range(0, 4);
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
            case 3:
                TeleportToBehind(playSound);
                break;
        }
    }

    public bool GetShouldQTE()
    {
        return shouldQTE;
    }

    public void SetShouldQTE(bool value)
    {
        shouldQTE = value;
    }

    public bool GetGotDeflected(){
        return gotDeflected;
    }

    public void SetGotDeflected(bool value){
        gotDeflected = value;
    }
}
