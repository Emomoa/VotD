using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private bool isColliding;
    private PlayerMovement playerMovement;

    [SerializeField]
    private AudioSource hit;
    [SerializeField]
    private AudioSource scrape;

    [SerializeField] private float n;
    [SerializeField] private float e;
    [SerializeField] private float s;
    [SerializeField] private float w;

    // Start is called before the first frame update
    void Start()
    {
        n = transform.position.x + (transform.lossyScale.x / 2);
        e = transform.position.z + (transform.lossyScale.z / 2);
        s = transform.position.x - (transform.lossyScale.x / 2);
        w = transform.position.z - (transform.lossyScale.z / 2);

        // Configure AudioSources
        if (scrape != null)
        {
            scrape.loop = true;
            scrape.playOnAwake = false;
        }

        if (hit != null)
        {
            hit.playOnAwake = false;
            hit.loop = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isColliding && playerMovement != null && playerMovement.isMoving)
        {
            Debug.Log("Player is moving and colliding: " + playerMovement.isMoving);
            if (!scrape.isPlaying)
            {
                Debug.Log("Scraping");
                scrape.Play();
            }
        }
        else
        {
            if (scrape.isPlaying)
            {
                scrape.Stop();
                Debug.Log("Scrape sound stopped.");
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision with: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Player"))
        {
            // Determine panning based on position
            if (collision.gameObject.transform.position.x > n || collision.gameObject.transform.position.x < s)
            {
                hit.panStereo = 0;
                scrape.panStereo = 0;
            }
            else
            {
                if (collision.gameObject.transform.position.z > e)
                {
                    hit.panStereo = 1;
                    scrape.panStereo = 1;
                }
                else
                {
                    hit.panStereo = -1;
                    scrape.panStereo = -1;
                }
            }

            // Play hit sound once
            if (hit != null)
            {
                hit.Play();
                Debug.Log("Hit sound played.");
            }

            // Set colliding state
            isColliding = true;

            // Assign playerMovement dynamically
            playerMovement = collision.transform.GetComponent<PlayerMovement>();
            if (playerMovement == null)
            {
                Debug.LogWarning("PlayerMovement component not found on the Player.");
            }
            else
            {
                Debug.Log("PlayerMovement component assigned.");
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("Collision ended with: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Player"))
        {
            isColliding = false;
            playerMovement = null; // Clear reference
        }
    }
}
