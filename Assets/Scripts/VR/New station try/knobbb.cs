using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class knobbb : XRBaseInteractable
{
    private XRBaseInteractor interactor;
    private bool isInteracting;
    private Quaternion initialRotation;

    public float rotationSpeed = 5f; // Adjust this value to control the rotation speed
    public float maxRotationAngle = 45f; // Maximum rotation angle

    protected override void Awake()
    {
        base.Awake();
        initialRotation = transform.localRotation;
    }

    protected override void OnSelectEntering(XRBaseInteractor interactor)
    {
        base.OnSelectEntering(interactor);
        this.interactor = interactor;
        isInteracting = true;
    }

    protected override void OnSelectExiting(XRBaseInteractor interactor)
    {
        base.OnSelectExiting(interactor);
        this.interactor = null;
        isInteracting = false;
    }

    private void Update()
    {
        if (isInteracting)
        {
            Rotate();
        }
    }

    private void Rotate()
    {
        // Get the rotation change from the controller
        Quaternion rotationDelta = GetRotationDelta();

        // Apply rotation only to the local rotation
        transform.localRotation *= rotationDelta;

        // Clamp the rotation to the specified angle range
        ClampRotation();
    }

    private Quaternion GetRotationDelta()
    {
        // Get the controller's rotation change
        Quaternion controllerRotationDelta = interactor.attachTransform.rotation * Quaternion.Inverse(initialRotation);

        // Extract the rotation around the X-axis
        float angle;
        Vector3 axis;
        controllerRotationDelta.ToAngleAxis(out angle, out axis);

        // Convert the angle to a rotation around the X-axis
        Quaternion rotationDelta = Quaternion.AngleAxis(angle, Vector3.right);

        return rotationDelta;
    }

    private void ClampRotation()
    {
        // Clamp the rotation to the specified angle range
        float currentAngle = transform.localRotation.eulerAngles.y;
        float clampedAngle = Mathf.Clamp(currentAngle, 0f, maxRotationAngle);

        // Apply the clamped rotation only to the local rotation
        transform.localRotation = Quaternion.Euler(clampedAngle, 0f, 0f);
    }
}
