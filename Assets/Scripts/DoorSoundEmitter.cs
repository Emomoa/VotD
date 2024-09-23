using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DoorSoundEmitter : MonoBehaviour
{
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.spatialBlend = 1f;
    }

    void OnEnable()
    {
        EventManager.OnCompassActivated += PlayCompassSound;
    }

    void OnDisable()
    {
        EventManager.OnCompassActivated -= PlayCompassSound;
    }

    void PlayCompassSound()
    {
        if (audioSource.clip != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("No AudioClip assigned to the door's AudioSource.");
        }
    }
}
