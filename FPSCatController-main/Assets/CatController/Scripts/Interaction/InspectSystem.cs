using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InspectSystem : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera; // FPS camera
    public Transform holdPosition; // Position to hold items
    public PlayerInput playerInput; // Assign in inspector

    [Header("Settings")]
    public float interactRange = 2f; // Range to detect objects
    public float throwForce = 10f; // Throw force
    public LayerMask interactableLayer; // Optional layer filter

    [Header("Events")]
    public UnityEvent onInteractableInRange; // Triggered when an interactable is in range
    public UnityEvent onInteractableOutOfRange; // Triggered when no interactable is in range

    private GameObject heldObject; // Currently held object
    private IInteractable currentInteractable; // Current interaction strategy
    private bool isHolding = false; // Track if holding
    private GameObject lastInteractableInRange; // Track the last object in range

    void Start()
    {
        if (holdPosition != null) return;

        GameObject holdObj = new GameObject("HoldPosition");
        holdObj.transform.SetParent(playerCamera.transform);
        holdObj.transform.localPosition = new Vector3(0, -0.2f, 0.5f);
        holdPosition = holdObj.transform;
    }

    void Update()
    {
        CheckInteractableInRange(); // Check range every frame
    }

    // Called via PlayerInput component (set up in Unity Inspector)
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed) return; // Only proceed on "performed" phase

        if (isHolding)
        {
            ReleaseObject();
            return;
        }

        TryPickupObject();
    }

    void CheckInteractableInRange()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        bool isInRange = Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableLayer) &&
                         hit.collider.TryGetComponent(out IInteractable _);

        GameObject currentInteractable = isInRange ? hit.collider.gameObject : null;

        if (currentInteractable != lastInteractableInRange)
        {
            if (isInRange && !isHolding)
            {
                onInteractableInRange?.Invoke();
            }
            else
            {
                onInteractableOutOfRange?.Invoke();
            }
            lastInteractableInRange = currentInteractable;
        }
    }

    void TryPickupObject()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if (!Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableLayer)) return;

        if (!hit.collider.TryGetComponent(out IInteractable interactable)) return;

        heldObject = hit.collider.gameObject;
        currentInteractable = interactable;

        currentInteractable.OnInspect(holdPosition);
        isHolding = true;

        heldObject.transform.position = holdPosition.position;
        heldObject.transform.rotation = holdPosition.rotation;
    }

    void ReleaseObject()
    {
        if (heldObject == null || currentInteractable == null) return;

        Vector3 throwDirection = playerCamera.transform.forward;
        currentInteractable.OnRelease(throwDirection, throwForce);

        heldObject = null;
        currentInteractable = null;
        isHolding = false;

        CheckInteractableInRange(); // Recheck range after releasing
    }

    void LateUpdate()
    {
        if (!isHolding || heldObject == null) return;

        heldObject.transform.position = holdPosition.position;
        heldObject.transform.rotation = holdPosition.rotation;
    }
}