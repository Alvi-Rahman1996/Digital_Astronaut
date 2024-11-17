using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Updated_sound_manager : MonoBehaviour
{
    public AudioSource[] audioSources;

    private bool soundEnabled = false;

    private void Start()
    {
        ToggleSound(); // Disable all sound sources initially
    }

    public void ToggleSound()
    {
        soundEnabled = !soundEnabled;

        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.mute = !soundEnabled;
        }
    }
}
