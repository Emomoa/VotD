using UnityEngine;
using UnityEngine.Events;

public class ActivationObject : MonoBehaviour
{
    public UnityEvent OnActivated; // Event triggered when this object is activated

    private bool playerInRange = false;
    private bool isActivated = false;

    // Reference to the Puzzle Controller
    public PuzzleController puzzleController;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            // Optionally, display UI prompt to press "E"
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            // Optionally, hide UI prompt
        }
    }

    void Update()
    {
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

        // Trigger any additional events
        OnActivated.Invoke();

        // Optionally, change the object's appearance to indicate activation
    }

    public void ResetActivation()
    {
        isActivated = false;
    }
}
