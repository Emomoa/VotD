using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSound : MonoBehaviour
{
    private AudioClip sound;
    public AudioSource source;


    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        sound = GetComponent<AudioClip>();

    }

    private void OnCollisionEnter(Collision collision)
    {
        source.Play();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
