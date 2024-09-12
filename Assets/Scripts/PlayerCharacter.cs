using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField]
    private float speed = 1f;
    [SerializeField]
    private bool IsSneaking = false;


    private bool IsInteracting = false;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Sneak()
    {
        speed = .5f;
        IsSneaking=true;
    }

    void Interact()
    {

    }
}
