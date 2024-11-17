using Photon.Pun;
using UnityEngine;

public class LeftHandJoystickRobotArm : MonoBehaviourPun
{
    public float rotationSpeed = 5f;
    public float movementSpeed = 5f;
    public Transform horizontalMovementObject;
    public Transform verticalMovementObject;
    
    private PhotonView photonView;
    private AudioSource armAudioSource; // Reference to the AudioSource component.

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        armAudioSource = GetComponent<AudioSource>(); // Assign the AudioSource component.
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        float rotationAmount = -horizontalInput * rotationSpeed * Time.deltaTime;
        float movementAmount = verticalInput * movementSpeed * Time.deltaTime;

        horizontalMovementObject.Rotate(0f, 0f, rotationAmount);
        verticalMovementObject.Rotate(0f, movementAmount, 0f);

        // Check if the arm is moving and play the sound.
        if (Mathf.Abs(rotationAmount) > 0f || Mathf.Abs(movementAmount) > 0f)
        {
            if (!armAudioSource.isPlaying)
            {
                armAudioSource.Play();
            }
        }
        else
        {
            // Stop the sound when the arm is not moving.
            armAudioSource.Stop();
        }

        photonView.RPC("SyncRobotArm", RpcTarget.Others, rotationAmount, movementAmount);
    }

    [PunRPC]
    private void SyncRobotArm(float rotationAmount, float movementAmount)
    {
        horizontalMovementObject.Rotate(0f, 0f, rotationAmount);
        verticalMovementObject.Rotate(0f, movementAmount, 0f);
    }
}
