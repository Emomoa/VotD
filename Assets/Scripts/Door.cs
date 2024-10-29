using System;

using UnityEngine.SceneManagement;
using UnityEngine;
using Random = UnityEngine.Random;

public class Door : MonoBehaviour
{
    public bool isOpen = false;
    public bool isClosed = false;

    public AudioSource audioSource;
    public AudioClip doorLocked;
    public AudioClip doorOpen;
    [SerializeField] private AudioClip creak;

    public string nextSceneName; // �ndra fr�n Scene till string

    public AudioSource[] pings;
    public int pingAmount = 3;

    float timeTillNextPing = 0;
    float pingInterval = 10f;
    int maxPings;
    int pingsUsed;
    float timePlayed;
    void Start()
    {
        maxPings = pingAmount;

    }
    void Update()
    {
        timePlayed += Time.deltaTime;

        if(Input.GetKeyDown("q"))
        {
            PingDoor();
        }
        if(pingAmount<maxPings)
        {
            timeTillNextPing += Time.deltaTime;
        }
        if(timeTillNextPing > pingInterval)
        {
            timeTillNextPing = 0;
            pingAmount++;
        }
        if (isOpen && !audioSource.isPlaying)
        {
            audioSource.clip = creak;
            audioSource.PlayDelayed(Random.Range(1f,4f));
        }
    }

    private void PingDoor()
    {
        int pingToPlay = UnityEngine.Random.Range(0,pings.Length);
        if(pingAmount>0)
        {
            Debug.Log("Player has pinged: " + pingsUsed + " times");
            if(pingAmount==1)
            {
                pings[pingToPlay].pitch *= 2;
            }
            pings[pingToPlay].Play();
            pingAmount--;
        }

    }

    public string sceneToLoadName;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Yolo");
        if (other.CompareTag("Player"))
        {
            if (isOpen)
            {
                Debug.Log("It took: " + timePlayed + " seconds for the player to complete the maze");
                // Spela upp ljudet f�r �ppnad d�rr
                audioSource.PlayOneShot(doorOpen);

                // Ladda n�sta scen med namn
                SceneManager.LoadScene(sceneToLoadName);
            }
            else if (isClosed)
            {
                // Spela upp ljudet f�r l�st d�rr
                audioSource.PlayOneShot(doorLocked);
            }
        }
    }

    public void Open()
    {
        audioSource.PlayOneShot(creak);
    }
}
