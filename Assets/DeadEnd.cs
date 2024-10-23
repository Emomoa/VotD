using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadEnd : MonoBehaviour
{
    public AudioSource source;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!source.isPlaying)
            {
                source.Play();
            }

        }
    }


    // Update is called once per frame
    void Update()
    {

    }
}
