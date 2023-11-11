// Reference : https://forum.unity.com/threads/force-camera-aspect-ratio-16-9-in-viewport.385541

using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    [Header("Resolution Settings"), Space, SerializeField]
    private float width = 19f;
    [SerializeField]
    private float height = 9f;

    private Camera cameraComponent;

    private void Awake()
    {
        cameraComponent = GetComponent<Camera>();

        SetupRect();
    }

#if !UNITY_EDITOR
    private void OnPreCull()
    {
        GL.Clear(true, true, Color.black);
    }
#endif

    private void SetupRect()
    {
        Rect rect = cameraComponent.rect;

        float targetAspect = width / height;
        float screenAspect = (float)Screen.width / Screen.height;
        float scaleHeight = screenAspect / targetAspect;
        float scaleWidth = 1f / scaleHeight;

        if (scaleHeight < 1f)
        {
            rect.height = scaleHeight;
            rect.y = (1f - scaleHeight) * 0.5f;
        }
        else
        {
            rect.width = scaleWidth;
            rect.x = (1f - scaleWidth) * 0.5f;
        }

        cameraComponent.rect = rect;
    }
}
