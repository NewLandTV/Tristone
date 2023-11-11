using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("UI Settings"), Space, SerializeField]
    private RectTransform handle;
    [SerializeField]
    private RectTransform background;

    // Handle radius
    private float radius;

    public Vector2 Input { get; private set; }

    private void Awake()
    {
        radius = background.rect.width * 0.5f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 value = eventData.position - (Vector2)background.position;

        Input = value.normalized;

        value = Vector2.ClampMagnitude(value, radius);

        handle.anchoredPosition = value;
    }

    public void OnPointerDown(PointerEventData eventData) { }

    public void OnPointerUp(PointerEventData eventData)
    {
        Input = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }
}
