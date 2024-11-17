using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Lever : XRBaseInteractable
{
    private XRBaseInteractor interactor;
    private bool isInteracting;
    private Quaternion initialRotation;
    private bool isReturning;

    public float minAngle = 45f; // Minimum rotation angle in degrees
    public float maxAngle = -45f; // Maximum rotation angle in degrees
    public float returnSpeed = 5f; // Speed at which the lever returns to its original position

    protected override void OnSelectEntering(XRBaseInteractor interactor)
    {
        base.OnSelectEntering(interactor);

        this.interactor = interactor;
        isInteracting = true;

        if (isReturning)
        {
            isReturning = false;
            initialRotation = transform.localRotation;
        }
    }

    protected override void OnSelectExiting(XRBaseInteractor interactor)
    {
        base.OnSelectExiting(interactor);

        this.interactor = null;
        isInteracting = false;
        isReturning = true;
    }

    protected override void Awake()
    {
        base.Awake();
        initialRotation = transform.localRotation;
    }

    private void Update()
    {
        if (isInteracting && interactor)
        {
            // Get the interactor's position relative to the lever's parent (the box)
            Vector3 interactorPosition = transform.parent.InverseTransformPoint(interactor.transform.position);

            // Calculate the target angle based on the interactor's position
            float targetAngle = Mathf.Lerp(maxAngle, minAngle, Mathf.InverseLerp(-1f, 1f, interactorPosition.y)) - 45f;

            // Apply the rotation to the lever around its local X-axis
            transform.localRotation = initialRotation * Quaternion.Euler(targetAngle, 0f, 0f);
        }
        else if (isReturning)
        {
            // Return the lever to its original position
            transform.localRotation = Quaternion.Lerp(transform.localRotation, initialRotation, returnSpeed * Time.deltaTime);
        }
    }
}
