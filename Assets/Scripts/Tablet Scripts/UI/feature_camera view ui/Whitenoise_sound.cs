using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whitenoise_sound : MonoBehaviour
{
    public AudioSource soundPlayer;
    public void play_whitenoise() 
    {
        soundPlayer.Play();
    }
}
