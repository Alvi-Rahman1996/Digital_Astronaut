using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_Manager : MonoBehaviour

{
    public AudioSource[] audioSources;

    private bool soundEnabled = true;

    public void ToggleSound()
    {
        soundEnabled = !soundEnabled;
        
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.mute = !soundEnabled;
        }
    }
}
