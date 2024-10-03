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

    private bool emmaBool = false;
    private bool emmaBool2 = false;
    // Start is called before the first frame update
    void Start()
    {
        AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && SceneManager.GetActiveScene().name == "PuzzleRoomElements" && emmaBool && !AudioSource.isPlaying)
        {
            AudioSource.clip = audioClip;
            AudioSource.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && !emmaBool2)
        {
            emmaBool2 = true;
            StartCoroutine(PuzzleRoomRiddle());
        }
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
        emmaBool = true;
    }
}
