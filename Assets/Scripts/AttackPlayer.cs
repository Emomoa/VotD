using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayer : MonoBehaviour
{
    [SerializeField]
    private float attackIntervall = 0;
    public float speed;
    private bool IsAttacking = false;

    [SerializeField]
    private GameObject Player;
    [SerializeField]
    private Transform PlayerTransform;

    private void Start()
    {
        // Call the function to spawn the ghost at a random position around the player
        Vector3 spawnPosition = GetRandomSpawnPosition();
        transform.position = spawnPosition;
    }

    private void Update()
    {
        // Example of calling ChasePlayer() to follow the player
        ChasePlayer();
    }

    // Function to chase the player
    private void ChasePlayer()
    {
        if (!IsAttacking)
        {
            // Logic to chase the player
        }
    }

    // Function to randomize the spawn position around the player
    private Vector3 GetRandomSpawnPosition()
    {
        // Get the player's position
        Vector3 playerPosition = PlayerTransform.position;

        // Define the distance from the player for the ghost to spawn
        float spawnDistance = 5f; // Adjust the spawn distance as needed

        // Randomly select a direction: 0 = front, 1 = behind, 2 = left, 3 = right
        int direction = Random.Range(0, 4);

        Vector3 spawnPosition = playerPosition;

        switch (direction)
        {
            case 0: // In front of the player
                spawnPosition = playerPosition + PlayerTransform.forward * spawnDistance;
                break;
            case 1: // Behind the player
                spawnPosition = playerPosition - PlayerTransform.forward * spawnDistance;
                break;
            case 2: // To the left of the player
                spawnPosition = playerPosition - PlayerTransform.right * spawnDistance;
                break;
            case 3: // To the right of the player
                spawnPosition = playerPosition + PlayerTransform.right * spawnDistance;
                break;
        }

        return spawnPosition;
    }
}
