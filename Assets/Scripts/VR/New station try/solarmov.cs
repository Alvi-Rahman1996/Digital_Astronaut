using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class solarmov : XRBaseInteractable
{
    private XRBaseInteractor interactor;
    private bool isInteracting;
    private Quaternion initialRotation;

    public float rotationSpeed = 50f; // Adjust as needed
    private bool isKnobActivated = false;

    public Transform parentEmptyObject; // Reference to the empty parent GameObject
    public Transform rotatingObject; // Reference to the object you want to rotate
    public InputHelpers.Button buttonToRotate = InputHelpers.Button.PrimaryButton; // Change to the desired button

    protected override void Awake()
    {
        base.Awake();
        initialRotation = transform.localRotation;
    }

    private void UpdateKnobRotation()
    {
        if (isInteracting)
        {
            float rotationAmount = -rotationSpeed * Time.deltaTime; // Invert the rotation for negative z-axis
            parentEmptyObject.Rotate(Vector3.forward, rotationAmount);

            // Rotate the other object based on the knob rotation
            if (rotatingObject != null)
            {
                rotatingObject.Rotate(Vector3.forward, rotationAmount); // Change to rotate around the z-axis
            }
        }
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        interactor = args.interactor;
        isInteracting = true;
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        interactor = null;
        isInteracting = false;
    }

    private void Update()
    {
        UpdateKnobRotation();
    }

    protected override void OnActivate(XRBaseInteractor interactor)
    {
        base.OnActivate(interactor);
        isKnobActivated = true;
    }

    protected override void OnDeactivate(XRBaseInteractor interactor)
    {
        base.OnDeactivate(interactor);
        isKnobActivated = false;
    }

    private void LateUpdate()
    {
        if (isKnobActivated)
        {
            // Rotate the parent empty GameObject based on knob rotation
            if (parentEmptyObject != null)
            {
                float rotationAmount = -rotationSpeed * Time.deltaTime; // Invert the rotation for negative z-axis
                parentEmptyObject.Rotate(Vector3.forward, rotationAmount);

                // Rotate the other object based on the knob rotation
                if (rotatingObject != null)
                {
                    rotatingObject.Rotate(Vector3.forward, rotationAmount); // Change to rotate around the z-axis
                }
            }
        }
    }
}
