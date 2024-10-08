using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TutorialWall : MonoBehaviour
{
    public int i;
    // Start is called before the first frame update
    void Start()
    {
        PlaySounds();
    }

    async void PlaySounds()
    {
        await Task.Delay(i);
        gameObject.GetComponentInChildren<AudioSource>().Play();

    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.GetComponentInChildren<Rigidbody>() == null)
        {
            
            gameObject.SetActive(false);
        }
    }
}
