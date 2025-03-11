using UnityEngine;

public class LetterItem : MonoBehaviour, IInteractable
{
    private Rigidbody rb;
    public GameObject letterPanel; // Assign in inspector or via a manager

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnInspect(Transform holdPosition)
    {
        rb.isKinematic = true;
        rb.useGravity = false;
        transform.position = holdPosition.position;
        transform.rotation = holdPosition.rotation;

        if (letterPanel != null)
        {
            letterPanel.SetActive(true);
        }
    }

    public void OnRelease(Vector3 throwDirection, float throwForce)
    {
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.AddForce(throwDirection * throwForce, ForceMode.Impulse);

        if (letterPanel != null)
        {
            letterPanel.SetActive(false);
        }
    }
}