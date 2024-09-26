using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crossroads : MonoBehaviour
{
    public AudioSource MainSource;
    public AudioSource RightSource;
    public AudioSource LeftSource;
    public AudioSource StraightSource;


    // Start is called before the first frame update
    void Start()
    {
        MainSource = GetComponent<AudioSource>();
        MainSource.Play();


    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlaySounds();

            
        }
            
    }

    void PlaySounds()
    {

        if (RightSource != null)
        {
            RightSource.Play();
        }

        if (StraightSource != null)
        {
            StraightSource.PlayDelayed(1);
        }
        if (LeftSource != null)
        {
            LeftSource.PlayDelayed(2);
        }
        

    }

    // Update is called once per frame
    void Update()
    {
        if (MainSource.isPlaying == null)
        {
            MainSource.Play();
        }
    }
}
