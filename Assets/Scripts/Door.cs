using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public bool isOpen = false;
    public bool isClosed = false;

    public AudioSource audioSource;
    public AudioClip doorLocked;
    public AudioClip doorOpen;

    public string nextSceneName; // Ändra från Scene till string

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Yolo");
        if (other.CompareTag("Player"))
        {
            if (isOpen)
            {
                // Spela upp ljudet för öppnad dörr
                audioSource.PlayOneShot(doorOpen);

                // Ladda nästa scen med namn
                SceneManager.LoadScene(nextSceneName);
            }
            else if (isClosed)
            {
                // Spela upp ljudet för låst dörr
                audioSource.PlayOneShot(doorLocked);
            }
        }
    }
}
