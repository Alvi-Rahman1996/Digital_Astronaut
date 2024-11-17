using UnityEngine;
using UnityEngine.UI;

public class UIPrefabScript : MonoBehaviour
{
    void Start()
    {
        // Find the button named "RoboticARM_Button" in the UI prefab
        Button flashButton = FindButton("RoboticARM_Button");

        if (flashButton != null)
        {
            // Add a listener to the button
            flashButton.onClick.AddListener(OnFlashButtonClick);
        }
        
    }

    void OnFlashButtonClick()
    {
        // Find the JointRotationController script in the scene
        JointRotationController jointRotationController = FindObjectOfType<JointRotationController>();

        if (jointRotationController != null)
        {
            // Call the OnRotateJointsButtonClick method
            jointRotationController.OnRotateJointsButtonClick();
        }
        else
        {
            Debug.LogError("JointRotationController script not found in the scene.");
        }
    }

    // Helper method to find a button by name
    Button FindButton(string buttonName)
    {
        Button[] buttons = GetComponentsInChildren<Button>(true);

        foreach (Button button in buttons)
        {
            if (button.gameObject.name == buttonName)
            {
                return button;
            }
        }

        return null;
    }
}
