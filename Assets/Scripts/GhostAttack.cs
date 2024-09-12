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
    private float speed = 10;

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

        if (shouldAttack)
        {
            Debug.LogWarning("Attacking");
            TimerCounter = 0;
            HandleAttack();
            shouldAttack = false;
        }

        if (shouldRunTowardPlayer)
        {
            if (player != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
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
