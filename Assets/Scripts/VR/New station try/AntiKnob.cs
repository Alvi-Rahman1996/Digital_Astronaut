using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AntiKnob : XRBaseInteractable
{
    private XRBaseInteractor interactor;
    private bool isInteracting;
    private Quaternion initialRotation;

    public float rotationSpeed = 50f; // Adjust as needed
    //public float movementSpeed = 0.1f; // Adjust as needed
    private bool isKnobActivated = false;

    public Transform knob; // Reference to the knob
    public Transform rotatingObject; // Reference to the object you want to rotate and move
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
            float rotationAmount = -rotationSpeed * Time.deltaTime; // Adjusted to rotate in positive z-axis

            // Rotate the knob based on the knob rotation
            if (knob != null)
            {
                knob.Rotate(Vector3.back, rotationAmount); // Change to rotate around the +z-axis
            }

            // Rotate the object based on the knob rotation
            if (rotatingObject != null)
            {
                rotatingObject.Rotate(Vector3.back, rotationAmount); // Change to rotate around the +z-axis
                // Move the object in the positive z-axis direction
                //rotatingObject.Translate(Vector3.back * movementSpeed * Time.deltaTime);
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
            // Rotate and move the object based on knob rotation
            if (knob != null && rotatingObject != null)
            {
                float rotationAmount = -rotationSpeed * Time.deltaTime; // Adjusted to rotate in positive z-axis

                // Rotate the knob based on the knob rotation
                knob.Rotate(Vector3.back, rotationAmount); // Change to rotate around the +z-axis

                // Rotate the object based on the knob rotation
                rotatingObject.Rotate(Vector3.back, rotationAmount); // Change to rotate around the +z-axis

                // Move the object in the positive z-axis direction
                //rotatingObject.Translate(Vector3.back * movementSpeed * Time.deltaTime);
            }
        }
    }
}
