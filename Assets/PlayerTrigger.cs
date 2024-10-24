using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    // This will be different for each child collider
    public string colliderDirection;

    public PlayerAttack playerAttack;

    void OnTriggerEnter(Collider other)
    {
        // Check if the ghost triggered the collider
        if (other.CompareTag("Ghost"))
        {
            switch (colliderDirection)
            {
                case "Front":
                    playerAttack.SetIsFront(true);
                    break;
                case "Back":
                    playerAttack.SetIsBack(true);
                    break;
                case "Left":
                    playerAttack.SetIsLeft(true);
                    break;
                case "Right":
                    playerAttack.SetIsRight(true);
                    break;
            }
        }
    }
}
