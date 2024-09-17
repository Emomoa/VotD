using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    [SerializeField] bool isLit;

    [SerializeField] AudioClip torchSound;
    [SerializeField] AudioClip lightTorchSound;
    [SerializeField] AudioClip putOutTorchSound;
    [SerializeField] AudioClip hitSound;

    [SerializeField] AudioSource audioSource;

    private PlayerMovement playerMovement;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if(audioSource == null) {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.clip = torchSound;
        audioSource.playOnAwake = true;
        audioSource.loop = true;

        if(isLit) {
            audioSource.Play();
        }
    }

    void Update()
    {
        // Hämtar in golvet spelaren står på.
        
        string groundTag = playerMovement.GetGroundTag();

        if (groundTag == "Carpet")
        {
            if (!isLit)
            {
                ToggleIsLit(true);
                Debug.Log("Fackla aktiverad");
            }
        }
        else if (groundTag == "Tile")
        {
            if (isLit)
            {
                ToggleIsLit(false);
                Debug.Log("Fackla deaktiverad");
            }
        }
        
    }

    public void ToggleIsLit(bool state) {
        isLit = state;
        if(isLit && !audioSource.isPlaying) {
            audioSource.Play();
        }
    }

    public void PlayHitSound() {
        if(hitSound != null  && audioSource != null) {
            audioSource.PlayOneShot(hitSound);
        }
    }

    private void LightWaypoint() {
        //när spelaren går förbi otänd fackla på vägg tänds den och ger ljudclue
    }
}
