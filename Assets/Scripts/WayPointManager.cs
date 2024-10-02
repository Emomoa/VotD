using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WayPointManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] wayPoints;
    [SerializeField]
    private AudioClip[] wayPointsAudio;
    private int currentPoint = 0;
    private AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        source = wayPoints[0].GetComponent<AudioSource>();
        source.clip = wayPointsAudio[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (!source.isPlaying && currentPoint<=wayPoints.Length-1)
        {
            PlaySound();
        }
    }

    public void NextPoint()
    {
        currentPoint += 1;
        if (currentPoint < wayPoints.Length) {
            source = wayPoints[currentPoint].GetComponent<AudioSource>();
            source.clip = wayPointsAudio[0];
        } else
        {
            //spela vinn-ljud
        }
        

    }

    private void PlaySound()
    {
        source.Play();
    }
}
