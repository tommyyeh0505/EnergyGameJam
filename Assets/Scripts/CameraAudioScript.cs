using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CameraAudioScript : MonoBehaviour
{
    private AudioSource musicSource;

    void Start()
    {
        musicSource = GetComponent<AudioSource>(); 
        if (musicSource)
        {
            musicSource.Play();
        }
    }

    public void Stop()
    {
        musicSource.Stop();
    }
}
