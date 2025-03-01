using UnityEngine;
using Cinemachine;

public class CatCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cinemachineCamera;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float cameraSensitivity = 2f;

    private float cameraPitch = 0f;
    private float characterYaw = 0f;

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
    }

    public void Rotate(Vector2 lookInput)
    {
        characterYaw += lookInput.x * cameraSensitivity * rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0f, characterYaw, 0f);

        cameraPitch -= lookInput.y * cameraSensitivity * rotationSpeed * Time.deltaTime;
        cameraPitch = Mathf.Clamp(cameraPitch, -90f, 90f);
        cinemachineCamera.transform.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
    }
}