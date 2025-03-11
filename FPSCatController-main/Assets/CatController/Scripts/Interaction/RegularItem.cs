using UnityEngine;

public class RegularItem : MonoBehaviour, IInteractable
{
    private Rigidbody rb;

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
    }

    public void OnRelease(Vector3 throwDirection, float throwForce)
    {
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.AddForce(throwDirection * throwForce, ForceMode.Impulse);
    }
}