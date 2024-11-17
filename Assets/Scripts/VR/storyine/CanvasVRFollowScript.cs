using UnityEngine;

public class CanvasVRFollowScript : MonoBehaviour
{
    private Transform vrCameraTransform;
    public float distance = 2.0f;

    private void Start()
    {
        vrCameraTransform = Camera.main.transform; // Assuming the Main Camera is the VR camera
    }

    private void LateUpdate()
    {
        if (vrCameraTransform != null)
        {
            Vector3 targetPosition = vrCameraTransform.position + vrCameraTransform.forward * distance;
            transform.position = targetPosition;
            transform.rotation = Quaternion.LookRotation(vrCameraTransform.forward, vrCameraTransform.up);
        }
    }
}
