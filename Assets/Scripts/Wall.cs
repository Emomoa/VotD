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
    [SerializeField] private float n;
    [SerializeField] private float e;
    [SerializeField] private float s;
    [SerializeField] private float w;

    // Start is called before the first frame update
    void Start()
    {
        n = gameObject.transform.position.x + (gameObject.transform.lossyScale.x / 2);
        e = gameObject.transform.position.z + (gameObject.transform.lossyScale.z / 2);
        s = gameObject.transform.position.x - (gameObject.transform.lossyScale.x / 2);
        w = gameObject.transform.position.z - (gameObject.transform.lossyScale.z / 2);

        //audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isColliding && playerMovement.isMoving)
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
