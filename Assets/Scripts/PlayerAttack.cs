using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip quickTimeEventQue;
    public AudioClip deflectedSound;
    public AudioClip swingTorchSound;
    public AudioSource audioSource;

    [Header("Variables")]
    [SerializeField] private float deflectWindowTime = 10f;
    private float deflectWindowTimer = 0f;

    private PlayerMovement playerMovement;
    private Torch torch;
    private GhostAttack ghost;

    private bool isDeflecting = false;
    private bool isLookingAtGhost = false;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        torch = GetComponent<Torch>();
        ghost = FindObjectOfType<GhostAttack>();
        
    }

    // Update is called once per frame
    void Update()
    {
        // Check if player is looking at ghost.
        RaycastHit hit = playerMovement.GetRaycasting();
        if(hit.collider != null)
        {
            if (hit.collider.name == "Ghost")
            {
                isLookingAtGhost = true;
                Debug.Log(isLookingAtGhost);
            }
            else
            {
                isLookingAtGhost = false;
            }
        }

        // check if ghost should activate QTE.
        if (ghost != null)
        {
            if (ghost.GetShouldQTE())
            {
                isDeflecting = true;
                HandleQTE();
            }
        }
    }

    async void HandleQTE()
    {
        if (isDeflecting)
        {
            deflectWindowTimer += Time.deltaTime;

            //Debug.LogWarning("WINDOW TIMER: " + deflectWindowTimer);
            //await Task.Delay(250);

            // Player deflected
            if (torch.GetIsLit() && isLookingAtGhost && Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Deflected ghost!");
                //torch.ToggleIsLit(false);
                audioSource.Stop();
                audioSource.PlayOneShot(swingTorchSound);

                // Play ghost scream sound
                ghost.GetComponent<AudioSource>().Stop();
                audioSource.PlayOneShot(deflectedSound);
                EndDeflectWindow();
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
    }

    void EndDeflectWindow()
    {
        isDeflecting = false;               // End the deflect window
        deflectWindowTimer = 0f;            // Reset the timer
        ghost.SetShouldQTE(false);          // End QTE.
        ghost.GetComponent<CapsuleCollider>().enabled = false;
    }

}
