using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public bool isOpen = false;
    public bool isClosed = false;

    public AudioSource audioSource;
    public AudioClip doorLocked;
    public AudioClip doorOpen;

    public string nextSceneName; // �ndra fr�n Scene till string

    public AudioSource[] pings;
    public int pingAmount = 3;
    void Update()
    {
        if(Input.GetKeyDown("q"))
        {
            PingDoor();
        }
    }

    private void PingDoor()
    {
        int pingToPlay = Random.Range(0,pings.Length);
        if(pingAmount>0)
        {
            if(pingAmount==1)
            {
                pings[pingToPlay].pitch *= 2;
            }
            pings[pingToPlay].Play();
            pingAmount--;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Yolo");
        if (other.CompareTag("Player"))
        {
            if (isOpen)
            {
                // Spela upp ljudet f�r �ppnad d�rr
                audioSource.PlayOneShot(doorOpen);

                // Ladda n�sta scen med namn
                SceneManager.LoadScene(nextSceneName);
            }
            else if (isClosed)
            {
                // Spela upp ljudet f�r l�st d�rr
                audioSource.PlayOneShot(doorLocked);
            }
        }
    }
}
