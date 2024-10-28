using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class PuzzleController : MonoBehaviour
{
    [Header("Activation Sequence")]
    public List<ActivationObject> activationSequence;



    public int currentActivationIndex = 0;
    private bool _puzzleSolved = false;
    [SerializeField]
    private AudioClip[] correctActivationSound;
    [SerializeField]
    private AudioClip[] incorrectActivationSound;
    [SerializeField]
    private AudioClip puzzleSolvedSound;

    [SerializeField]
    private AudioClip interactExplain;

    private AudioSource puzzleSolvedSource;

    public GameObject doorToUnlock;

    private int _counter;
    private int _resetCounter;

    [Header("Events")]
    public UnityEvent OnPuzzleSolved;
    public UnityEvent OnIncorrectActivation;

    void Start()
    {
        SetCurrentPriority(activationSequence[currentActivationIndex]);
        puzzleSolvedSource = GetComponent<AudioSource>();
        // Initialize activation objects and assign this controller
        foreach (var obj in activationSequence)
        {
            obj.puzzleController = this;
            obj.ResetActivation();
        }
        doorToUnlock.SetActive(false);
    }

    public void ObjectActivated(ActivationObject activatedObject)
    {
        if (_puzzleSolved)
            return;

        if (activationSequence[currentActivationIndex] == activatedObject)
        {
            // Correct object activated
            currentActivationIndex++;
            if (puzzleSolvedSource.isPlaying)
            {
                puzzleSolvedSource.Stop();
                puzzleSolvedSource.PlayOneShot(correctActivationSound[Random.Range(0, correctActivationSound.Length - 1)]);
                
                //Kolla över detta!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                
                // if (currentActivationIndex >= activationSequence.Count - 1)
                // {
                //     SetCurrentPriority(activationSequence[currentActivationIndex]); 
                // }
                
            }
            
            activatedObject.GetComponent<AudioSource>().Stop();


            if (currentActivationIndex >= activationSequence.Count)
            {
                // Puzzle solved
                _puzzleSolved = true;
                OnPuzzleSolved.Invoke();
                puzzleSolvedSource.PlayOneShot(puzzleSolvedSound);
                Debug.Log("Puzzle Solved!");
            }
        }
        else
        {
            // Incorrect object activated
            OnIncorrectActivation.Invoke();
            if (puzzleSolvedSource.isPlaying) 
            {
                puzzleSolvedSource.Stop();
                puzzleSolvedSource?.PlayOneShot(incorrectActivationSound[_counter]);
            }
            
            if (_counter < incorrectActivationSound.Length - 1)
            {
                _counter += 1;
            }
            Debug.Log("Incorrect activation. Resetting puzzle.");
            ResetPuzzle();
        }
    }

    public void ResetPuzzle()
    {
        currentActivationIndex = 0;
        SetCurrentPriority(activationSequence[currentActivationIndex]);
        foreach (var obj in activationSequence)
        {
            obj.ResetActivation();
            obj.GetComponent<AudioSource>().Play();
            _resetCounter++;
            Debug.Log("Reset counter: " + _resetCounter);
        }
    }

    public void InteractionSound()
    {
        puzzleSolvedSource.clip = interactExplain;
        puzzleSolvedSource.Play();
    }

    public void SetCurrentPriority(ActivationObject currentPrio)
    {
        var currentAudioSource = currentPrio.GetComponent<AudioSource>();
        currentAudioSource.priority = 50;
        currentAudioSource.volume = 1f;

    }

    public void UnlockDoor()
    {
        doorToUnlock.SetActive(true);
    }
}
