using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LeverNew : XRBaseInteractable
{
    private XRBaseInteractor interactor;
    private bool isInteracting;
    private Quaternion initialRotation;
    private bool isDown; // Track whether the lever is currently down or not

    public float downAngle = 90f; // Angle when the lever is pressed down
    public float upAngle = -45f; // Angle when the lever is released

    protected override void OnSelectEntering(XRBaseInteractor interactor)
    {
        base.OnSelectEntering(interactor);
        this.interactor = interactor;
        isInteracting = true;

        // Toggle the lever position
        isDown = !isDown;
        SetLeverAngle();
    }

    protected override void OnSelectExiting(XRBaseInteractor interactor)
    {
        base.OnSelectExiting(interactor);
        this.interactor = null;
        isInteracting = false;
    }

    protected override void Awake()
    {
        base.Awake();
        initialRotation = transform.localRotation;
        isDown = false; // Start with the lever in the up position
        SetLeverAngle();
    }

    private void SetLeverAngle()
    {
        float targetAngle = isDown ? downAngle : upAngle;
        transform.localRotation = initialRotation * Quaternion.Euler(targetAngle, 0f, 0f);
    }
}
