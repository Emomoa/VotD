using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GhostTutorial : MonoBehaviour
{
    public GameObject ghost;
    public BoxCollider activateGhostCollider;

    void OnTriggerEnter(Collider other){
        // Collides with player
        if(other.CompareTag("Player")){
            ghost.SetActive(true);
        }
    }

}
