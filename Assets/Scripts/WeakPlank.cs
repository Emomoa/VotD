using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class WeakPlank : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement playerMove;
    [SerializeField]
    private PlayerAttack playerAttack;
    public AudioSource audioSource;
    public AudioClip creakingSound;
    public AudioClip plankBreakSound;
    public float breakTimer = 0.5f; // Duration before the plank breaks

    [SerializeField]
    private GameObject parentGO;

    [SerializeField]
    private GameObject weakPlankPrefab;

    private Coroutine breakCoroutine;
    private bool isPlayerOnPlank = false;
    private bool isBreaking = false;


    public delegate void PlankBreakHandler();
    public static event PlankBreakHandler OnPlankDestroy;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerOnPlank = true;
            if (!playerMove.isSneaking)
            {
                StartBreaking();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerOnPlank = false;
            StopBreaking();
        }
    }

    void Update()
    {
        if (isPlayerOnPlank)
        {
            if (playerAttack.deflected)
            {
                breakCoroutine = StartCoroutine(InstantBreak());
                return;
            }
            if (playerMove.isSneaking && isBreaking)
            {
                // Player started sneaking while on the plank
                StopBreaking();
            }
            else if (!playerMove.isSneaking && !isBreaking)
            {
                // Player stopped sneaking while on the plank
                StartBreaking();
            }
        }
    }

    private void StartBreaking()
    {
        if (isBreaking) return;
        isBreaking = true;
        // Play creaking sound
        audioSource.clip = creakingSound;
        audioSource.loop = true;
        audioSource.Play();
        // Start the break timer
        breakCoroutine = StartCoroutine(BreakPlank());
    }

    private void StopBreaking()
    {
        if (!isBreaking) return;
        isBreaking = false;
        // Stop creaking sound
        audioSource.Stop();
        // Stop the break timer
        if (breakCoroutine != null)
        {
            StopCoroutine(breakCoroutine);
            breakCoroutine = null;
        }
    }

    private IEnumerator BreakPlank()
    {
        yield return new WaitForSeconds(breakTimer);
        audioSource.loop = false; // Set loop to false before stopping
        audioSource.Stop();
        var clip = audioSource.clip;
        clip = null;
        clip = plankBreakSound;
        audioSource.clip = clip;
        audioSource.Play();
        yield return new WaitForSeconds(clip.length);
    
        // Break the plank
        isBreaking = false;
        Debug.Log("The plank has broken!");
    
        // Kill the player
        playerMove.Die();
        OnPlankDestroy?.Invoke();
        
        // Destroy the plank
        // Destroy(parentGO);
    }

    private IEnumerator InstantBreak()
    {
        audioSource.loop = false; // Set loop to false before stopping
        audioSource.Stop();
        var clip = audioSource.clip;
        clip = null;
        clip = plankBreakSound;
        audioSource.clip = clip;
        audioSource.Play();
        yield return new WaitForSeconds(clip.length);

        // Break the plank
        isBreaking = false;
        Debug.Log("The plank has broken!");

        // Kill the player
        playerMove.Die();
        OnPlankDestroy?.Invoke();

        // Destroy the plank
        // Destroy(parentGO);
    }
}
