using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSounds : MonoBehaviour
{/*
    public AudioSource source;
    public AudioClip clip;
    public AudioClip[] sounds;
    private bool bumpNoice = false;
    // Start is called before the first frame update

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collison");
        if (collision.gameObject.tag == "Player" && bumpNoice == false)
        {
            Debug.LogWarning("vägg");
            bumpNoice = true;
            source.PlayOneShot(clip);
        }
        else
        {
            return;
        }
    }*/

    // Update is called once per frame
    /*void Update()
    {
        if (bumpNoice == true && collision.gameObject.tag == "Player")
        {
            if (source.isPlaying == false)
            {
                source.PlayOneShot(sounds[0]);
            }

        }
        else
        {
            bumpNoice = false;
        }
    }*/
}
