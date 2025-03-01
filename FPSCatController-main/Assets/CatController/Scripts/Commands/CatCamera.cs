using UnityEngine;
using Cinemachine;

public class CatCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cinemachineCamera;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float cameraSensitivity = 2f;
    [SerializeField] private Transform characterTransform; // Reference to CatRoot

    private float cameraPitch = 0f;

    void Awake()
    {
        if (cinemachineCamera == null)
        {
            cinemachineCamera = GetComponentInChildren<CinemachineVirtualCamera>();
            if (cinemachineCamera == null)
            {
                Debug.LogError("CinemachineVirtualCamera not found in CatCamera!");
            }
        }
        if (characterTransform == null)
        {
            characterTransform = transform.parent; // Assume parent is CatRoot
            if (characterTransform == null)
            {
                Debug.LogError("Character Transform not assigned in CatCamera!");
            }
        }
    }

    public void Rotate(Vector2 lookInput)
    {
        // Horizontal rotation (yaw) - Rotate the character (CatRoot)
        float yaw = lookInput.x * cameraSensitivity * rotationSpeed * Time.deltaTime;
        characterTransform.Rotate(0f, yaw, 0f);

        // Vertical rotation (pitch) - Rotate the camera locally
        cameraPitch -= lookInput.y * cameraSensitivity * rotationSpeed * Time.deltaTime;
        cameraPitch = Mathf.Clamp(cameraPitch, -90f, 90f);
        cinemachineCamera.transform.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
    }
}