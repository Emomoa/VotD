using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAttack : MonoBehaviour
{

    public GameObject player;
    public AudioSource attackStarted;
    public AudioSource runTowardPlayer;

    [Tooltip("How fast the ghost runs toward the player")]
    [SerializeField]
    private float speed = 10f;

    [Tooltip("How close the ghost gets to the player")]
    [SerializeField]
    private float stopDistance = 2f;

    [Tooltip("How far the ghost is from the player before attacking")]
    [SerializeField]
    private float offset = 10;

    [Tooltip("Every ** seconds the ghost attacks.")]
    [SerializeField]
    private float AttackInterval = 10;

    private float TimerCounter = 0;
    private float TimerIncreaseRate = 1;

    private bool shouldAttack = false;
    private bool shouldRunTowardPlayer = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Debug
        if (Input.GetKeyDown(KeyCode.T))
        {
            shouldAttack = true;
        }

        //Debug.Log(TimerCounter);
        // Increase/update timer.
        TimerCounter += TimerIncreaseRate * Time.fixedDeltaTime;

        // If timer reaches attackinterval, attack.
        if(TimerCounter >= AttackInterval)
        {
            
        }

        // Attacks player
        if (shouldAttack)
        {
            Debug.LogWarning("Attacking");
            TimerCounter = 0;
            HandleAttack();
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
                    // Move the object towards the player's position
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                }
                else
                {
                    runTowardPlayer.Stop();
                    runTowardPlayer.loop = false;
                    shouldRunTowardPlayer = false;
                    Debug.Log("Stopped moving! Close enough to the player.");
                }
            }

        }
        
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

            attackStarted.Play();
        }
    }
    void TeleportToLeft()
    {
        // Move to left of player
        if (player != null)
        {
            //Debug.Log("Teleporting...");
            Vector3 playerPosition = player.transform.position;
            Vector3 newPosition = playerPosition + player.transform.right * -offset;
            transform.position = newPosition;

            attackStarted.Play();

            Invoke("RunTowardPlayer", 3);
        }
    }

    void HandleAttack()
    {
        TeleportToRight();
        //increasingSound.PlayDelayed(2);

        Invoke("TeleportToLeft", 2);
    }

    void RunTowardPlayer()
    {
        runTowardPlayer.Play();
        shouldRunTowardPlayer = true;

    }
}
