// Reference : https://srk911028.tistory.com/122

using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    [Header("Settings"), Space, SerializeField]
    private bool controllable;
    public bool Controllable { get => controllable; set => controllable = value; }

    private float halfScreenWidth;

    // Target finger id
    private int rightFingerId = -1;

    public Vector2 Input { get; private set; }

    private void Awake()
    {
        halfScreenWidth = Screen.width >> 1;
    }

    private void Update()
    {
        if (controllable)
        {
            GetTouchInput();
        }
    }

    private void GetTouchInput()
    {
        for (int i = 0; i < UnityEngine.Input.touchCount; i++)
        {
            Touch touch = UnityEngine.Input.GetTouch(i);

            switch (touch.phase)
            {
                // Began input the right finger
                case TouchPhase.Began:
                    if (touch.position.x > halfScreenWidth && rightFingerId == -1)
                    {
                        rightFingerId = touch.fingerId;
                    }

                    break;
                case TouchPhase.Moved:
                    if (EventSystem.current.IsPointerOverGameObject(i))
                    {
                        break;
                    }

                    if (touch.fingerId != rightFingerId)
                    {
                        break;
                    }

                    // Yaw, Pitch
                    Vector2 previousPoint = touch.position - touch.deltaPosition;

                    Input = new Vector2(touch.position.x - previousPoint.x, touch.deltaPosition.y);

                    break;
                case TouchPhase.Stationary:
                    if (touch.fingerId == rightFingerId)
                    {
                        Input = Vector2.zero;
                    }

                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (touch.fingerId == rightFingerId)
                    {
                        Input = Vector2.zero;
                        rightFingerId = -1;
                    }

                    break;
            }
        }
    }
}
