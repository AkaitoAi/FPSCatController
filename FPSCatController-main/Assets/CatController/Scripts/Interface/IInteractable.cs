using UnityEngine;

public interface IInteractable
{
    void OnInspect(Transform holdPosition); // Called when picked up
    void OnRelease(Vector3 throwDirection, float throwForce); // Called when thrown or released
}