using UnityEngine;

public class BlinkingLight : MonoBehaviour
{
    public Light bulbLight; // Reference to the Light component

    public float minIntensity = 1.0f; // Minimum light intensity
    public float maxIntensity = 5.0f; // Maximum light intensity
    public float glowSpeed = 1.0f; // Speed of the glowing effect

    private float targetIntensity;

    private void Start()
    {
        // Start the glowing effect when entering play mode
        targetIntensity = minIntensity; // Start with the minimum intensity
        InvokeRepeating("ToggleGlow", 0.0f, glowSpeed);
    }

    private void ToggleGlow()
    {
        // Toggle between min and max intensity to create the glowing effect
        targetIntensity = (targetIntensity == minIntensity) ? maxIntensity : minIntensity;
        bulbLight.intensity = targetIntensity;
    }
}
