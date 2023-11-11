using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ControlButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Callback Functions"), Space, SerializeField]
    private UnityEvent onPointerDownEvent;
    [SerializeField]
    private UnityEvent onPointerUpEvent;

    public void OnPointerDown(PointerEventData eventData)
    {
        onPointerDownEvent?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        onPointerUpEvent?.Invoke();
    }
}
