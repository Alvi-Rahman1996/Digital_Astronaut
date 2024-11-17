using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Updated_Sound_Toggle : MonoBehaviour
{
    private bool soundEnabled = true;
    private Sound_Manager soundManager;
    private Image buttonImage;

    private void Start()
    {
        soundManager = FindObjectOfType<Sound_Manager>();

        buttonImage = GetComponent<Image>();
        buttonImage.color = soundEnabled ? Color.white : Color.red;

        Button button = GetComponent<Button>();
        button.onClick.AddListener(ToggleSound);
    }

    private void ToggleSound()
    {
        soundEnabled = !soundEnabled;
        buttonImage.color = soundEnabled ? Color.white : Color.red;

        soundManager.ToggleSound();
    }
}
