using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TutScript : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private bool isLookingAtTutSound = false;
    private AudioSource source;
    public int timer;
    private AudioClip clip;
    public AudioClip[] clips;
    private int lol = 0;


    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        source = GetComponent<AudioSource>();
        PlaySounds();

    }

    async void PlaySounds()
    {
        
        await Task.Delay(timer);
        source.PlayOneShot(clips[lol]);
        lol++;

    }

    // Update is called once per frame
    void Update()
    {
        if(source.isPlaying == false) {
        RaycastHit hit = playerMovement.GetRaycasting();
            if (hit.collider != null)
            {
                if (hit.collider.tag == "TutSound")
                {
                    isLookingAtTutSound = true;
                    hit.collider.gameObject.SetActive(false);
                    Debug.Log(isLookingAtTutSound);
                }
                else
                {
                    isLookingAtTutSound = false;
                }
            }
        }
    }
}
