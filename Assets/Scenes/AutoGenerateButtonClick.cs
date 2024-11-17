using UnityEngine;
using UnityEngine.UI;

public class AutoGenerateButtonClick : MonoBehaviour
{
    void Start()
    {
        // Find the button with the specified name in the UI hierarchy
        Button yourButton = GameObject.Find("Buttonforarm").GetComponent<Button>();

        // Find the specified GameObject in the scene
        GameObject targetGameObject = GameObject.Find("Roboticarm");

        // Specify the function name
        string functionName = "OnRotateJointsButtonClick()";

        // Ensure the button reference is set
        if (yourButton != null)
        {
            // Automatically generate onClick event during play mode
            yourButton.onClick.AddListener(() => { ExecuteOnClickEvent(targetGameObject, functionName); });
        }
    }

    void ExecuteOnClickEvent(GameObject targetGameObject, string functionName)
    {
        // Ensure the target GameObject is set
        if (targetGameObject != null)
        {
            // Find the specified function on the target GameObject and invoke it
            targetGameObject.SendMessage(functionName);
        }
    }
}
