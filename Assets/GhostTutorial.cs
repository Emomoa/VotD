using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GhostTutorial : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip Intro;
    public AudioClip TorchIntro;
    public AudioClip DeflectSound;

    // Update is called once per frame
    async void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(Intro);
            }
            
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(TorchIntro);
                await Task.Delay(17500);
                audioSource.PlayOneShot(DeflectSound);
            }

        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(DeflectSound);
            }

        }

    }
}
