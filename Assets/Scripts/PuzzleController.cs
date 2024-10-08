using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class PuzzleController : MonoBehaviour
{
    [Header("Activation Sequence")]
    public List<ActivationObject> activationSequence; // The correct sequence of activation objects

    private int currentActivationIndex = 0;
    private bool puzzleSolved = false;
    [SerializeField]
    private AudioClip correctActivationSound;
    [SerializeField]
    private AudioClip incorrectActivationSound;
    [SerializeField]
    private AudioClip puzzleSolvedSound;

    [Header("Events")]
    public UnityEvent OnPuzzleSolved;
    public UnityEvent OnIncorrectActivation;

    void Start()
    {
        // Initialize activation objects and assign this controller
        foreach (var obj in activationSequence)
        {
            obj.puzzleController = this;
            obj.ResetActivation();
        }
    }

    public void ObjectActivated(ActivationObject activatedObject)
    {
        if (puzzleSolved)
            return;

        if (activationSequence[currentActivationIndex] == activatedObject)
        {
            // Correct object activated
            currentActivationIndex++;


            if (currentActivationIndex >= activationSequence.Count)
            {
                // Puzzle solved
                puzzleSolved = true;
                OnPuzzleSolved.Invoke();
                Debug.Log("Puzzle Solved!");
            }
        }
        else
        {
            // Incorrect object activated
            OnIncorrectActivation.Invoke();
            Debug.Log("Incorrect activation. Resetting puzzle.");
            ResetPuzzle();
        }
    }

    public void ResetPuzzle()
    {
        currentActivationIndex = 0;
        foreach (var obj in activationSequence)
        {
            obj.ResetActivation();
        }
    }
}
