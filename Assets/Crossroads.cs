using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crossroads : MonoBehaviour
{

    public AudioSource RightSource;
    public AudioSource LeftSource;
    public AudioSource StraightSource;


    // Start is called before the first frame update
    void Start()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
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
    }

    // Update is called once per frame
    void Update()
    {

    }
}
