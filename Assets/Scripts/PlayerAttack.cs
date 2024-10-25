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
    public AudioClip[] hitSuccess;
    public AudioSource audioSource;

    public bool canDeflect = false;
    public bool deflected = false;

    [Header("Variables")]
    [SerializeField] private float deflectWindowTime = 10f;
    private float deflectWindowTimer = 0f;

    private PlayerMovement playerMovement;
    private Torch torch;
    private GhostAttack ghostAttack;

    private bool isFront;
    private bool isBack;
    private bool isLeft;
    private bool isRight;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        torch = GetComponent<Torch>();
        ghostAttack = FindObjectOfType<GhostAttack>();
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

    }

    bool HandleInput(){
        if(isFront && Input.GetKey(KeyCode.UpArrow))
        {
            return true;
        }
        else if(isBack && Input.GetKey(KeyCode.DownArrow)){
            return true;
        }
        else if(isLeft && Input.GetKey(KeyCode.LeftArrow))
        {
            return true;
        }
        else if (isRight && Input.GetKey(KeyCode.RightArrow))
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
        if (torch.GetIsLit() && HandleInput() && Input.GetKeyDown(KeyCode.Space) && canDeflect )
        {
            Debug.Log("Deflected ghost!");
            EndDeflectWindow();
            //torch.ToggleIsLit(false);
            audioSource.Stop();
            audioSource.PlayOneShot(swingTorch);
            deflected = true;
            await Task.Delay(100);
            ghostAttack.SetGotDeflected(true);
            audioSource.PlayOneShot(hitSuccess[Random.Range(0,4)]);
            deflected = false;
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
        ResetDirection();

    }

    void ResetDirection(){
        isFront = false;
        isBack = false; 
        isLeft = false;
        isRight = false;
    }
    public void SetIsFront(bool value){
        isFront = value;
    }

    public void SetIsBack(bool value)
    {
        isBack = value;
    }

    public void SetIsLeft(bool value)
    {
        isLeft = value;
    }

    public void SetIsRight(bool value)
    {
        isRight = value;
    }
}
