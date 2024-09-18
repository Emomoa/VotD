using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class WeakPlank : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement playerMove;
    public AudioSource audioSource;
    public AudioClip creakingSound;
    public AudioClip plankBreakSound;
    public float breakTimer = 3f; // Duration before the plank breaks

    [SerializeField]
    private GameObject parentGO;

    private Coroutine breakCoroutine;
    private bool isPlayerOnPlank = false;
    private bool isBreaking = false;

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
        // Break the plank
        isBreaking = false;
        audioSource.Stop();
        audioSource.loop = false;
        audioSource.clip = plankBreakSound;
        audioSource.Play();
        Debug.Log("The plank has broken!");
        // Kill the player
        playerMove.Die();
        parentGO.SetActive(false);
        parentGO.SetActive(true);
    }
}
