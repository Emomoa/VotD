using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStartAudio : MonoBehaviour
{
    [SerializeField] private AudioClip dialogue;
    [SerializeField] private AudioClip soundEffect;
    [SerializeField] private AudioSource audioSource1;
    [SerializeField] private AudioSource audioSource2;
    // Start is called before the first frame update
    void Start()
    {
        if (soundEffect != null)
        {
            audioSource1.clip = soundEffect;
            audioSource1.Play();
        }



        if (dialogue != null)
        {
            audioSource2.clip = dialogue;
            audioSource2.PlayDelayed(soundEffect.length - audioSource1.time);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
