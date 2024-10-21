using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    [SerializeField] bool isLit;
    [SerializeField] float torchLitDelay = 1f;

    [SerializeField] AudioClip torchSound;
    [SerializeField] AudioClip lightTorchSound;
    [SerializeField] AudioClip putOutTorchSound;
    [SerializeField] AudioClip hitSound;

    [SerializeField] AudioSource audioSource;

    private PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();

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

    public bool GetIsLit(){
        return isLit;
    }

    public void ToggleIsLit(bool state) {
        isLit = state;
        if(!isLit){
            audioSource.Stop();
            audioSource.PlayOneShot(putOutTorchSound);
        }
        
        if(isLit) {
            audioSource.PlayOneShot(lightTorchSound);
            audioSource.PlayDelayed(torchLitDelay);
            Debug.Log("tänd fackla");
        }
        
    }

    public void PlayHitSound() {
        if(hitSound != null  && audioSource != null) {
            audioSource.PlayOneShot(hitSound);
        }
    }

}
