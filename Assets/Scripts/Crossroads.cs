using System.Collections;
using UnityEngine;

public class Crossroads : MonoBehaviour
{
    public AudioSource MainSource;
    public AudioSource RightSource;
    public AudioSource LeftSource;
    public AudioSource StraightSource;
    public AudioSource BackSource;

    private Coroutine soundCoroutine; // To keep track of the coroutine
    private bool playerInTrigger = false; // Flag to check if player is in the trigger

    void Start()
    {
        MainSource = GetComponent<AudioSource>();
        MainSource.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !playerInTrigger)
        {
            playerInTrigger = true;
            soundCoroutine = StartCoroutine(PlaySoundSequence());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && playerInTrigger)
        {
            playerInTrigger = false;

            if (soundCoroutine != null)
            {
                StopCoroutine(soundCoroutine);
                soundCoroutine = null;
            }
        }
    }

    IEnumerator PlaySoundSequence()
    {
        while (playerInTrigger)
        {
            if (RightSource != null)
            {
                RightSource.Play();
            }

            yield return new WaitForSeconds(1f); // Wait for 1 second

            if (StraightSource != null)
            {
                StraightSource.Play();
            }

            yield return new WaitForSeconds(1f); // Wait for 1 second

            if (LeftSource != null)
            {
                LeftSource.Play();
            }

            yield return new WaitForSeconds(1f); // Wait for 1 second

            if (BackSource != null)
            {
                BackSource.Play();
            }

            yield return new WaitForSeconds(1f);

        }
    }

    void Update()
    {
        if (!MainSource.isPlaying)
        {
            MainSource.Play();
        }
    }
}
