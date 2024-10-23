using System.Collections;
using UnityEngine;

public class Crossroads : MonoBehaviour
{
    public AudioSource MainSource;
    public AudioSource RightSource;
    public AudioSource LeftSource;
    public AudioSource StraightSource;
    public AudioSource BackSource;

    public bool RightSourceBool;
    public bool LeftSourceBool;
    public bool StraightSourceBool;
    public bool BackSourceBool;

    private Coroutine soundCoroutine; // To keep track of the coroutine
    private bool playerInTrigger = false; // Flag to check if player is in the trigger


    void CheckBools()
    {
        if (!RightSourceBool)
        {
            RightSource.enabled = false;
        } else { RightSource.enabled = true; }
        if (!LeftSourceBool)
        {
            LeftSource.enabled = false;
        } else {  LeftSource.enabled = true; }
        if (!StraightSourceBool)
        {
            StraightSource.enabled = false;
        } else {  StraightSource.enabled = true; }
        if (!BackSourceBool) 
        { 
            BackSource.enabled = false; 
        } else {  BackSource.enabled = true; }


    }
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

            yield return new WaitForSeconds(1.2f); // Wait for 1 second

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
        CheckBools();
        if (!MainSource.isPlaying)
        {
            MainSource.Play();
        }
    }
}
