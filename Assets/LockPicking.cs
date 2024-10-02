using System.Collections;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class LockPicking : MonoBehaviour
{
    public AudioSource beacon; // glöm inte att byta ut, dem låter lite för lika
    public AudioSource ping;
    public AudioSource lockTurn;
    public AudioSource errorSound;

    private bool inLockPick = false;
    public int locksToPick = 5;

    private bool temp = true; // byt namn till något mer lämpligt

    void Update()
    {
        if(inLockPick)
        {
            if(temp)
            {   // detta ska kanse vara i en metod som typ GenerateLock() // ska den ta svårighet?  
                StartCoroutine(PlayPings());
                pinPosition = Random.Range(-lockPickBarLength/5,lockPickBarLength/5);
                pinPosition = lockPickBarLength/2 + pinPosition;
                temp = false;
            }
            else // väntar på nästa frame 
            {
                Lockpinging();
            }
            

        }

        
    }
    public float LockPickMaxSpeed = 1;
    public AudioSource lockPickingSound;
    private float lockPickBarLength = 100;
    private float pickPosition = 0;

    private float pinPosition;

    
    
    private void Lockpinging()
    {
        float mousex = Input.GetAxis("Mouse X");
        float mouseSensForLockPick = 123; // bör vara cirka (90-max150)
        float moveSpeed = mousex*Time.deltaTime*mouseSensForLockPick; 

        if(moveSpeed>LockPickMaxSpeed)
        {
            moveSpeed = LockPickMaxSpeed;
        }
        else if(moveSpeed<-LockPickMaxSpeed)
        {
            moveSpeed = -LockPickMaxSpeed;
        }

        if(pickPosition > lockPickBarLength)
        {
            pickPosition = 0 + 0.3f;
        }
        else if(pickPosition < 0)
        {

            pickPosition = lockPickBarLength - 0.3f;

        }
        
        lockPickingSound.enabled = true; // den måste loopa

        lockPickingSound.maxDistance = 1; // om problem med ljud, testa att ta bort
        
        float pickMoveSpeed = 40f;
        pickPosition += moveSpeed*pickMoveSpeed*Time.deltaTime;

        float pickingVolume = moveSpeed;
        if(pickingVolume<0)
        {
            pickingVolume*=-1;

        }
        lockPickingSound.volume = pickingVolume;

        float lockPickSolveMargin = 7; // denna kan ändras ifall man vill påverka svårigheten
        if((pickPosition <= pinPosition+lockPickSolveMargin) && (pickPosition >= pinPosition-lockPickSolveMargin))
        {
            lockPickingSound.pitch = 3;

            if((pickPosition <= pinPosition+lockPickSolveMargin/2) && (pickPosition >= pinPosition-lockPickSolveMargin/2))
            {
                ping.Play();

            }

            if(Input.GetKeyDown("space"))
            {
                print("RÄTT");
                lockTurn.Play();
            }
        }
        else
        {
            lockPickingSound.pitch = 1;
            if(Input.GetKeyDown("space"))
            {
                print("Fel");
                errorSound.Play();
                // ev gå tillbaka ett steg i hur många som måste lösas. 
            }
        }
        print("Pick position: " + pickPosition + " | min rätt pos: " + (pinPosition-lockPickSolveMargin) + " | max rätt pos: "+ pinPosition+lockPickSolveMargin);
    


    }

    private IEnumerator PlayPings()// metod för att berätta hur många som behöver lösas
    {
        for (int i = 0; i < locksToPick; i++)
        {
            ping.Play(); 
            yield return new WaitForSeconds(ping.clip.length); 
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(!inLockPick)
        if (other.CompareTag("Player"))
        {
            inLockPick = true;   
            beacon.enabled = false;
            Debug.Log("Found lock");
            other.transform.GetComponent<PlayerMovement>().enabled = false;
            // vi behöver något som förklarar att man har tagit sig till ett lås

        }

    }

}
