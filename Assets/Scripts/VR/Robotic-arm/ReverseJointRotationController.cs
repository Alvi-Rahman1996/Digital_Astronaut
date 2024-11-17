using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ReverseJointRotationController : MonoBehaviour
{
    public Transform joint1_Y; // First vertical joint
    public Transform joint2_Y; // Second vertical joint

    public AudioSource armMovementSound;

    
    private float initialRotationJoint1_Y;
    private float initialRotationJoint2_Y;

    private float targetRotationJoint1_Y;
    private float targetRotationJoint2_Y;

    private bool isRotatingJoint1_Y = true;
    private bool isRotatingJoint2_Y;

    private Transform astronautTransform;
    private bool isHoldingAstronaut = true;
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

        astronautTransform = GameObject.FindGameObjectWithTag("Astronaut").transform;

        targetRotationJoint1_Y = initialRotationJoint1_Y - 130f;
        targetRotationJoint2_Y = initialRotationJoint2_Y - 55f;
    }

    void Update()
    {
        if (isHoldingAstronaut)
        {
            astronautTransform.rotation = astronautInitialRotation;
        }

        if (rotateJointsButtonPressed)
        {
            // Rotate the joints sequentially
            RotateJoints();
        }

        
    }


    void RotateJoints()
    {
        if (isRotatingJoint1_Y)
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

   
        }
    }
}