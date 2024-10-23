using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleRoomTrigger : MonoBehaviour
{
    private AudioSource AudioSource;
    [SerializeField]
    private AudioClip audioClip;

    [SerializeField]
    private AudioClip puzzleHintSound;

    [SerializeField]
    private AudioClip emmaTutorial;

    [SerializeField] private PuzzleController puzzleController;
    [SerializeField] private AudioClip[] clues;

    private bool _emmaBool = false;
    private bool _emmaBool2 = false;
    // Start is called before the first frame update
    private void Start()
    {
        AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && SceneManager.GetActiveScene().name == "PuzzleRoomElements" && _emmaBool && !AudioSource.isPlaying)
        {
            AudioSource.clip = clues[puzzleController.currentActivationIndex];
            AudioSource.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player") || _emmaBool2) return;
        _emmaBool2 = true;
        StartCoroutine(PuzzleRoomRiddle());
    }

    private IEnumerator PuzzleRoomRiddle ()
    {

        AudioSource.Play();
        yield return new WaitForSeconds(AudioSource.clip.length);
        AudioSource.clip = audioClip;
        AudioSource.Play();
        yield return new WaitForSeconds(AudioSource.clip.length + .5f);
        AudioSource.clip = puzzleHintSound;
        AudioSource.Play();
        yield return new WaitForSeconds(AudioSource.clip.length + .5f);
        AudioSource.clip = emmaTutorial;
        AudioSource.Play();
        yield return new WaitForSeconds(AudioSource.clip.length);
        _emmaBool = true;
    }
}
