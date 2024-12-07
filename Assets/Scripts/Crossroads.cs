using System.Collections;
using Unity.VisualScripting;
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
        if(MainSource!=null)
        {
            MainSource.Play();
        } 
        TestIfBeaconsShouldChangeRotation();
    }
    public float tempRotation;
    void TestIfBeaconsShouldChangeRotation()
    {
        tempRotation = transform.eulerAngles.y;
        if(transform.eulerAngles.y>1)
        {
            UpdateCrossroadCompass(false);
        }
        else if( transform.eulerAngles.y<-1)
        {
            UpdateCrossroadCompass(true);
        }

    }
    void UpdateCrossroadCompass(bool changeClockwise)
    {
        bool tempStraightSourceBool = StraightSourceBool;
        bool tempRightSourceBool = RightSourceBool;
        bool tempBackSourceBool = BackSourceBool;
        bool tempLeftSourceBool = LeftSourceBool;
        if(changeClockwise)
        {   // norr blir öst, öst, blir south, south blir west, west blir north
            
            LeftSourceBool = tempStraightSourceBool;
            BackSourceBool = tempLeftSourceBool;
            RightSourceBool = tempBackSourceBool;
            StraightSourceBool = tempRightSourceBool;
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x,transform.eulerAngles.y+90,transform.eulerAngles.z);
        }
        else
        {
            RightSourceBool = tempStraightSourceBool;
            BackSourceBool = tempRightSourceBool;
            LeftSourceBool = tempBackSourceBool;
            StraightSourceBool = tempLeftSourceBool; 
            
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x,transform.eulerAngles.y-90,transform.eulerAngles.z);
        }

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

            yield return new WaitForSeconds(1.5f); // Wait for 1 second

            if (StraightSource != null)
            {
                StraightSource.Play();
            }

            yield return new WaitForSeconds(1.5f); // Wait for 1 second

            if (LeftSource != null)
            {
                LeftSource.Play();
            }

            yield return new WaitForSeconds(1.5f); // Wait for 1 second

            if (BackSource != null)
            {
                BackSource.Play();
            }

            yield return new WaitForSeconds(1.5f);

        }
    }

    void Update()
    {
        CheckBools();
        if(MainSource == null)
        {
            return;
        }
        if (!MainSource.isPlaying)
        {
            MainSource.Play();
        }
    }
}
