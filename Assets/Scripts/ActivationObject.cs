using UnityEngine;
using UnityEngine.Events;

public class ActivationObject : MonoBehaviour
{
    public UnityEvent OnActivated; // Event triggered when this object is activated

    private bool playerInRange = false;
    private bool isActivated = false;
    private int _totalInteractPresses;

    // Reference to the Puzzle Controller
    public PuzzleController puzzleController;

    public UnityEvent onTriggerEnter;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isActivated)
        {
            playerInRange = true;
            onTriggerEnter.Invoke();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            _totalInteractPresses++;
            Debug.Log(_totalInteractPresses);
        }
        if (playerInRange && !isActivated && Input.GetKeyDown(KeyCode.E))
        {
            Activate();
        }
    }

    void Activate()
    {
        isActivated = true;
        // Notify the Puzzle Controller
        if (puzzleController != null)
        {
            puzzleController.ObjectActivated(this);
        }

        OnActivated.Invoke();
        
    }

    public void ResetActivation()
    {
        isActivated = false;
    }
}
