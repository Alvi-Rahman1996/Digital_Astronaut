using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundToggle : MonoBehaviour
{
    private bool soundEnabled = true;
    private bool isFirstClick = true; // New variable to track the first click
    private Sound_Manager soundManager;
    private Image buttonImage;

    private void Start()
    {
        soundManager = FindObjectOfType<Sound_Manager>();

        buttonImage = GetComponent<Image>();

        // Check if the game is in play mode
        if (Application.isPlaying)
        {
            if (isFirstClick)
            {
                ToggleSound(); // Automated first click
                isFirstClick = false; // Disable automation for subsequent clicks
            }

            buttonImage.color = soundEnabled ? Color.white : Color.red;
            Button button = GetComponent<Button>();
            button.onClick.AddListener(ToggleSound);
        }
    }

    private void ToggleSound()
    {
        soundEnabled = !soundEnabled;
        buttonImage.color = soundEnabled ? Color.white : Color.red;
        soundManager.ToggleSound();
    }
}
