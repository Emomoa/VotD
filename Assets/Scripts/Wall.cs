using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    //private AudioSource audioSource;
    private bool isColliding;
    private PlayerMovement playerMovement;
    [SerializeField]
    private AudioSource hit;
    [SerializeField]
    private AudioSource scrape;

    // Start is called before the first frame update
    void Start()
    {
        //audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isColliding && playerMovement.moveDirection.magnitude > 0.1f)
        {
            if (!scrape.isPlaying)
            {
                Debug.Log("Scraping");
                //audioSource.pitch = playerMovement.moveDirection.magnitude/5;
                scrape.Play();
            }
        } /*else
        {
            scrape.Stop();
        }*/
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision");
        if (collision.gameObject.CompareTag("Player"))
        {
            //audioSource.Play();
            Debug.Log("Player collision");
            hit.Play();
            isColliding = true;
            playerMovement = collision.transform.GetComponent<PlayerMovement>();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("No Collision");
        if (collision.gameObject.CompareTag("Player"))
        {
            isColliding = false;
        }
    }
}
