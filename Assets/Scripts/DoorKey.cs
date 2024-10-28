using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKey : MonoBehaviour
{

    [SerializeField] private GameObject door;
    [SerializeField] private AudioClip ongoing;
    [SerializeField] private AudioClip keyTaken;
    private bool isTaken = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTaken){
            door.GetComponent<Door>().isOpen = true;
            door.GetComponent<Door>().isClosed = false;
            gameObject.GetComponent<AudioSource>().loop = false;
            gameObject.GetComponent<AudioSource>().clip = keyTaken;
            gameObject.GetComponent<AudioSource>().Play();
            door.GetComponent<Door>().Open();
            isTaken = true;

            //gameObject.GetComponent<AudioSource>().Stop();
        }

    }
}
