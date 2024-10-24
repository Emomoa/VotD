using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip quickTimeEventQue;
    public AudioClip swingTorch;
    public AudioClip heartbeat;
    public AudioSource audioSource;

    [Header("Variables")]
    [SerializeField] private float deflectWindowTime = 10f;
    private float deflectWindowTimer = 0f;

    private PlayerMovement playerMovement;
    private Torch torch;
    private GhostAttack ghost;

    public bool canDeflect = false;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        torch = GetComponent<Torch>();
        ghost = FindObjectOfType<GhostAttack>();
        //ghostDeflectedSound = ghost.deflectedSound;

    }

    // Update is called once per frame
    void Update()
    {
        if (ghost.GetShouldQTE())
        {
            canDeflect = true;
            HandleQTE();
        }
        // Check if player is looking at ghost.

        // check if ghost should activate QTE.

    }
    bool HandleInput(){
        if(ghost.isFront && Input.GetKey(KeyCode.UpArrow))
        {
            return true;
        }
        else if(ghost.isBehind && Input.GetKey(KeyCode.DownArrow)){
            return true;
        }
        else if(ghost.isLeft && Input.GetKey(KeyCode.LeftArrow))
        {
            return true;
        }
        else if (ghost.isRight && Input.GetKey(KeyCode.RightArrow))
        {
            return true;
        }
        else return false;
    }

    async void HandleQTE()
    {
        deflectWindowTimer += Time.deltaTime;

        //Debug.LogWarning("WINDOW TIMER: " + deflectWindowTimer);
        //await Task.Delay(250);

        // Player deflected
        if (torch.GetIsLit() && HandleInput() && Input.GetKeyDown(KeyCode.Space) && canDeflect)
        {
            Debug.Log("Deflected ghost!");
            EndDeflectWindow();
            //torch.ToggleIsLit(false);
            audioSource.Stop();
            audioSource.PlayOneShot(swingTorch);
            await Task.Delay(100);
            ghost.SetGotDeflected(true);
            
            //torch.ToggleIsLit(true);
            ghost.ResetAttack();

        }
        else if (deflectWindowTimer >= deflectWindowTime)
        {
            // Time is up, end the deflect window
            Debug.Log("Deflect window expired!");
            EndDeflectWindow();
            ghost.ResetAttack();
            playerMovement.Die();
        }
    }

    void EndDeflectWindow()
    {
        canDeflect = false;                 // End the deflect
        deflectWindowTimer = 0f;            // Reset the timer
        ghost.SetShouldQTE(false);          // End QTE.
        ghost.GetComponent<CapsuleCollider>().enabled = false;
    }

}
