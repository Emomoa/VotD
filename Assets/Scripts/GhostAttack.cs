using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAttack : MonoBehaviour
{

    public GameObject player;
    public AudioSource audioSource;
    public AudioClip attackStartedSound;
    public AudioClip increasingSound;

    [Tooltip("Every ** seconds the ghost attacks.")]
    [SerializeField]
    private float AttackInterval = 10;

    private float TimerCounter = 0;
    private float TimerIncreaseRate = 1;

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
            TeleportToRight();
        }

        //Debug.Log(TimerCounter);
        // Increase/update timer.
        TimerCounter += TimerIncreaseRate * Time.fixedDeltaTime;

        // If timer reaches attackinterval, attack.
        if(TimerCounter >= AttackInterval)
        {
            //Debug.LogWarning("Attacking");
            TimerCounter = 0;
            TeleportToRight();
            attackStartedSound.Play();
            //increasingSound.PlayDelayed(2);
        }
        
    }

    void TeleportToRight()
    {
        // Move to right of player
        if (player != null)
        {
            //Debug.Log("Teleporting...");
            Vector3 playerPosition = player.transform.position;
            Vector3 newPosition = playerPosition + player.transform.right * 10;
            transform.position = newPosition;
            
        }
    }
}
