using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private WeakPlank weakPlank;
    [SerializeField]
    private AudioSource audioSource;

    private void OnEnable()
    {
        WeakPlank.OnPlankDestroy += PlayPlankBreakSound;
    }

    private void OnDisable()
    {
        WeakPlank.OnPlankDestroy -= PlayPlankBreakSound;
    }

    private void PlayPlankBreakSound()
    {
        // Play the plank break sound
        audioSource.Play();
    }
}
