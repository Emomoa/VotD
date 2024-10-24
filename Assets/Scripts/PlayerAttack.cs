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
    private GameObject ghost;
    private GhostAttack ghostAttack;

    public bool canDeflect = false;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        torch = GetComponent<Torch>();
        ghostAttack = FindObjectOfType<GhostAttack>();
        ghost = GameObject.FindGameObjectWithTag("Ghost");
        //ghostDeflectedSound = ghost.deflectedSound;

    }

    // Update is called once per frame
    void Update()
    {
        if (ghostAttack.GetShouldQTE())
        {
            canDeflect = true;
            HandleQTE();
        }

        // Calculate the vector from the player to the ghost
        Vector3 playerToGhost = ghost.transform.position - transform.position;

        // Normalize the direction
        playerToGhost.Normalize();

        // Get the player's forward and right directions
        Vector3 playerForward = transform.forward;
        Vector3 playerRight = transform.right;

        // Calculate the dot products
        float dotForward = Vector3.Dot(playerForward, playerToGhost);  // How much in front or behind
        float dotRight = Vector3.Dot(playerRight, playerToGhost);      // How much to the right or left


    }
    bool HandleInput(){
        if(ghostAttack.isFront && Input.GetKey(KeyCode.UpArrow))
        {
            return true;
        }
        else if(ghostAttack.isBehind && Input.GetKey(KeyCode.DownArrow)){
            return true;
        }
        else if(ghostAttack.isLeft && Input.GetKey(KeyCode.LeftArrow))
        {
            return true;
        }
        else if (ghostAttack.isRight && Input.GetKey(KeyCode.RightArrow))
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
            ghostAttack.SetGotDeflected(true);
            
            //torch.ToggleIsLit(true);
            ghostAttack.ResetAttack();

        }
        else if (deflectWindowTimer >= deflectWindowTime)
        {
            // Time is up, end the deflect window
            Debug.Log("Deflect window expired!");
            EndDeflectWindow();
            ghostAttack.ResetAttack();
            playerMovement.Die();
        }
    }

    void EndDeflectWindow()
    {
        canDeflect = false;                 // End the deflect
        deflectWindowTimer = 0f;            // Reset the timer
        ghostAttack.SetShouldQTE(false);          // End QTE.
        ghostAttack.GetComponent<CapsuleCollider>().enabled = false;
    }

}
