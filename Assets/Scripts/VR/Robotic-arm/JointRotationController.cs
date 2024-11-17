using UnityEngine;

public class JointRotationController : MonoBehaviour
{
    public Transform joint1_Y; // First vertical joint
    public Transform joint2_Y; // Second vertical joint
    public Transform joint_Z;  // Joint rotating on Z-axis
    public Transform joint4_Z;  // New joint rotating on Z-axis

    public AudioSource armMovementSound;

    private float initialRotationJoint1_Y;
    private float initialRotationJoint2_Y;
    private float initialRotationJoint_Z;
    private float initialRotationJoint4_Z;

    private float targetRotationJoint1_Y;
    private float targetRotationJoint2_Y;
    private float targetRotationJoint_Z;
    private float targetRotationJoint4_Z;

    private bool isRotatingJoint_Z = true;
    private bool isRotatingJoint1_Y;
    private bool isRotatingJoint2_Y;
    private bool isRotatingJoint4_Z;

    private Transform astronautTransform;
    private bool isHoldingAstronaut = false;
    private Transform robotArmEndTransform;
    private Transform astronautOriginalParent;
    private Quaternion astronautInitialRotation;

    public float rotationSpeed = 10f; // Adjust the speed of rotation

    private bool isInitialRotationsCompleted = false;
    private float timeSinceInitialRotationsCompleted;

    private bool rotateJointsButtonPressed = false;

    public void OnRotateJointsButtonClick()
    {
        rotateJointsButtonPressed = true;
    }

    void Start()
    {
        // Store the initial rotations
        initialRotationJoint1_Y = joint1_Y.localRotation.eulerAngles.y;
        initialRotationJoint2_Y = joint2_Y.localRotation.eulerAngles.y;
        initialRotationJoint_Z = joint_Z.localRotation.eulerAngles.z;

        // Initialize astronaut-related variables
        astronautTransform = GameObject.FindGameObjectWithTag("Astronaut").transform;

        // Initialize the new joint only if it's not assigned in the inspector
        if (joint4_Z == null)
        {
            joint4_Z = transform.Find("Joint4_Z"); // Adjust the name or hierarchy as needed
        }

        // Set the target rotations for each joint
        targetRotationJoint_Z = initialRotationJoint_Z - 90f;
        targetRotationJoint1_Y = initialRotationJoint1_Y - 130f;
        targetRotationJoint2_Y = initialRotationJoint2_Y - 65f;
        targetRotationJoint4_Z = joint4_Z.localRotation.eulerAngles.z - 30f;
    }


    void Update()
    {
        // Check if the astronaut is not currently being held
        if (!isHoldingAstronaut)
        {
            // Check if the robotic arm end is available
            if (robotArmEndTransform != null)
            {
                // Pick up the astronaut
                PickUpAstronaut();
            }
        }

        // Update the position of the astronaut while keeping the initial rotation
        if (isHoldingAstronaut)
        {
            astronautTransform.rotation = astronautInitialRotation;
        }

        // Check if the rotate joints button is pressed
        if (rotateJointsButtonPressed)
        {
            // Rotate the joints sequentially
            RotateJoints();

            // Check if the initial rotations are completed
            if (!isInitialRotationsCompleted && !isRotatingJoint_Z && !isRotatingJoint1_Y && !isRotatingJoint2_Y && !isRotatingJoint4_Z)
            {
                isInitialRotationsCompleted = true;
                timeSinceInitialRotationsCompleted = Time.time;
            }

            // Perform additional rotations after a delay
            if (isInitialRotationsCompleted)
            {
                float elapsedTime = Time.time - timeSinceInitialRotationsCompleted;

                // Add a delay of 2 seconds
                if (elapsedTime >= 2f)
                {
                    // Rotate the y-axis joints one by one for additional angles
                    RotateAdditionalYAxisJoints();

                    // Rotate the 4th joint on the Z-axis by -30f
                    RotateFourthZAxisJoint();
                }
            }
        }
    }

    void RotateJoints()
    {
        if (isRotatingJoint_Z)
        {
            // Calculate the new Z rotation for the joint rotating on Z-axis based on time
            float newZRotationJoint_Z = Mathf.MoveTowardsAngle(joint_Z.localRotation.eulerAngles.z, targetRotationJoint_Z, Time.deltaTime * rotationSpeed);

            // Apply the new rotation
            joint_Z.localRotation = Quaternion.Euler(0f, 0f, newZRotationJoint_Z);

            // Check if the rotation is complete
            if (Mathf.Approximately(newZRotationJoint_Z, targetRotationJoint_Z))
            {
                isRotatingJoint_Z = false; // Switch to the next joint
                isRotatingJoint1_Y = true;

                
            }

            if (armMovementSound != null && !armMovementSound.isPlaying)
                armMovementSound.Play();
        }
        else if (isRotatingJoint1_Y)
        {
            // Calculate the new Y rotation for the first vertical joint based on time
            float newYRotationJoint1_Y = Mathf.MoveTowardsAngle(joint1_Y.localRotation.eulerAngles.y, targetRotationJoint1_Y, Time.deltaTime * rotationSpeed);

            // Apply the new rotation
            joint1_Y.localRotation = Quaternion.Euler(0f, newYRotationJoint1_Y, 0f);

            // Check if the rotation is complete
            if (Mathf.Approximately(newYRotationJoint1_Y, targetRotationJoint1_Y))
            {
                isRotatingJoint1_Y = false; // Switch to the next joint
                isRotatingJoint2_Y = true;

                if (armMovementSound != null && !armMovementSound.isPlaying)
                    armMovementSound.Play();
            }
        }
        else if (isRotatingJoint2_Y)
        {
            // Calculate the new Y rotation for the second vertical joint based on time
            float newYRotationJoint2_Y = Mathf.MoveTowardsAngle(joint2_Y.localRotation.eulerAngles.y, targetRotationJoint2_Y, Time.deltaTime * rotationSpeed);

            // Apply the new rotation
            joint2_Y.localRotation = Quaternion.Euler(0f, newYRotationJoint2_Y, 0f);

            // Check if the rotation is complete
            if (Mathf.Approximately(newYRotationJoint2_Y, targetRotationJoint2_Y))
            {
                isRotatingJoint2_Y = false; // Switch to the next joint
                isRotatingJoint4_Z = true; // Activate the 4th joint rotation

                if (armMovementSound != null && !armMovementSound.isPlaying)
                    armMovementSound.Play();
            }
        }
        else if (isRotatingJoint4_Z)
        {
            // Calculate the new Z rotation for the 4th joint based on time
            float newZRotationJoint4_Z = Mathf.MoveTowardsAngle(joint4_Z.localRotation.eulerAngles.z, targetRotationJoint4_Z, Time.deltaTime * rotationSpeed);

            // Apply the new rotation
            joint4_Z.localRotation = Quaternion.Euler(0f, 0f, newZRotationJoint4_Z);

            // Check if the rotation is complete
            if (Mathf.Approximately(newZRotationJoint4_Z, targetRotationJoint4_Z))
            {
                isRotatingJoint4_Z = false; // All joints have completed their rotations

                if (armMovementSound != null && armMovementSound.isPlaying)
                    armMovementSound.Stop();
            }
        }
    }

    // Additional rotations after the initial rotations are completed
    void RotateAdditionalYAxisJoints()
    {
        // Rotate the first vertical joint by +95f
        float targetRotationJoint1_Y_Additional = initialRotationJoint1_Y + 45f;
        float newYRotationJoint1_Y_Additional = Mathf.MoveTowardsAngle(joint1_Y.localRotation.eulerAngles.y, targetRotationJoint1_Y_Additional, Time.deltaTime * rotationSpeed);
        joint1_Y.localRotation = Quaternion.Euler(0f, newYRotationJoint1_Y_Additional, 0f);

        // Rotate the second vertical joint by +180f
        float targetRotationJoint2_Y_Additional = initialRotationJoint2_Y + 55f;
        float newYRotationJoint2_Y_Additional = Mathf.MoveTowardsAngle(joint2_Y.localRotation.eulerAngles.y, targetRotationJoint2_Y_Additional, Time.deltaTime * rotationSpeed);
        joint2_Y.localRotation = Quaternion.Euler(0f, newYRotationJoint2_Y_Additional, 0f);

        // Play sound when additional rotation starts
        if (armMovementSound != null && !armMovementSound.isPlaying)
            armMovementSound.Play();

        // Check if both additional rotations are complete
        if (Mathf.Approximately(newYRotationJoint1_Y_Additional, targetRotationJoint1_Y_Additional)
            && Mathf.Approximately(newYRotationJoint2_Y_Additional, targetRotationJoint2_Y_Additional))
        {
            // Stop sound when additional rotations complete
            if (armMovementSound != null && armMovementSound.isPlaying)
                armMovementSound.Stop();
        }
    }


    // Rotate the 4th joint on the Z-axis by -30f
    void RotateFourthZAxisJoint()
    {
        float targetRotationJoint4_Z_Additional = initialRotationJoint4_Z + 80f;
        float newZRotationJoint4_Z_Additional = Mathf.MoveTowardsAngle(joint4_Z.localRotation.eulerAngles.z, targetRotationJoint4_Z_Additional, Time.deltaTime * rotationSpeed);
        joint4_Z.localRotation = Quaternion.Euler(0f, 0f, newZRotationJoint4_Z_Additional);
    }

    // ... existing PickUpAstronaut and DetachAstronaut methods ...

    private void PickUpAstronaut()
    {
        astronautOriginalParent = astronautTransform.parent;
        Debug.Log(astronautOriginalParent.name);

        // Store the initial rotation of the astronaut
        astronautInitialRotation = astronautTransform.rotation;

        // Attach the astronaut to the robotic arm end
        astronautTransform.SetParent(robotArmEndTransform);
        astronautTransform.GetComponent<Rigidbody>().isKinematic = true;

        isHoldingAstronaut = true;
    }

    private void DetachAstronaut()
    {
        // Detach the astronaut from the robotic arm end
        astronautTransform.SetParent(astronautOriginalParent);
        astronautTransform.GetComponent<Rigidbody>().isKinematic = false;

        isHoldingAstronaut = false;
    }
}
