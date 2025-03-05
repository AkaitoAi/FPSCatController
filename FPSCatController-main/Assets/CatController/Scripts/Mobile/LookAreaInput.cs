using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class LookAreaInput : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private Vector2 dragDelta;
    private bool isDragging;
    public Vector2 LookInput => isDragging ? dragDelta : Vector2.zero;

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        dragDelta = Vector2.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            dragDelta = eventData.delta;
        }
    }
}