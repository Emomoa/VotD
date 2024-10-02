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
    public AudioSource[] pinSounds;

    private bool inLockPick = false;
    public int locksToPick = 5;

    private bool temp = true; // byt namn till något mer lämpligt
    
    float pingTimer = 100;
    void Update()
    {
        if(inLockPick)
        {
            pingTimer += Time.deltaTime;

            if(pingTimer>1f)
            {
                if(temp)
                {   // detta ska kanse vara i en metod som typ GenerateLock() // ska den ta svårighet?  
                    StartCoroutine(PlayPings());
                    pinPosition = Random.Range(-lockPickBarLength/4,lockPickBarLength/4);
                    pinPosition = lockPickBarLength/2 + pinPosition;
                    temp = false;
                    pickPosition = 2;
                }
                else // väntar på nästa frame 
                {
                    Lockpinging();
                }
            }
            
            

        }

        
    }
    public float LockPickMaxSpeed = 1;
    public AudioSource lockPickingSound;
    private float lockPickBarLength = 100;
    private float pickPosition = 0;

    private float pinPosition;

    private float pinTimer = 0;
    
    
    private void Lockpinging()
    {
        pinTimer+=Time.deltaTime;

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
        
        float pickMoveSpeed = 28f;
        pickPosition += moveSpeed*pickMoveSpeed*Time.deltaTime;

        float pickingVolume = moveSpeed;
        if(pickingVolume<0)
        {
            pickingVolume*=-1;

            if((pickingVolume>0.05f)&&(pickingVolume<0.35f))
            {
                pickingVolume = 0.35f;
            }

        }
        lockPickingSound.volume = pickingVolume;

        float lockPickSolveMargin = 7; // denna kan ändras ifall man vill påverka svårigheten
        if((pickPosition <= pinPosition+lockPickSolveMargin) && (pickPosition >= pinPosition-lockPickSolveMargin))
        {
            

            if((pickPosition <= pinPosition+lockPickSolveMargin/4) && (pickPosition >= pinPosition-lockPickSolveMargin/4))
            {
                if(pickingVolume>0.1f)
                {
                    if(pinTimer>0.65f)
                    {
                        pinSounds[Random.Range(0,pinSounds.Length)].Play();
                        pinTimer = 0;
                    }
                    
                }
                
            }


            lockPickingSound.pitch = 1.8f;
            lockPickingSound.volume *= 1.2f;


            if(Input.GetKeyDown("space"))
            {
                print("RÄTT");
                temp = true;
                lockTurn.Play();
                pingTimer = 0;
                // gå fram ett steg i hur många lås som behöver lösas om siffran inte blir 0, för då vinner man 
            }
        }
        else
        {
            lockPickingSound.pitch = 0.8f;
            if(Input.GetKeyDown("space"))
            {
                print("Fel");
                errorSound.Play();
                
                pingTimer = 0;
                temp = true;
                // gå tillbaka ett steg i hur många lås som behöver låsas
            }
        }
        //print("Pick position: " + pickPosition + " | min rätt pos: " + (pinPosition-lockPickSolveMargin) + " | max rätt pos: "+ pinPosition+lockPickSolveMargin);
    


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
