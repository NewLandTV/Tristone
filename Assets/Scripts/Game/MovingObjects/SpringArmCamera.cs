// Reference : https://github.com/MohitSethi99/SpringArmComponent

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

[ExecuteAlways]
public class SpringArmCamera : MovingObject
{
    [Header("Follow Settings"), Space, SerializeField]
    private Transform target;
    [SerializeField]
    private float movementSmoothTime = 0.05f;
    [SerializeField]
    private float targetArmLength = 3f;
    [SerializeField]
    private Vector3 socketOffset;
    [SerializeField]
    private Vector3 targetOffset;

    [Space, Header("Collision Settings"), Space, SerializeField]
    private bool doCollisionTest = true;
    [Range(2, 20), SerializeField]
    private int collisionTestResolution = 4;
    [SerializeField]
    private float collisionProbeSize = 0.3f;
    [SerializeField]
    private float collisionSmoothTime = 0.05f;
    [SerializeField]
    private LayerMask collisionLayerMask = ~0;

    [Space, Header("Rotation Settings"), Space, SerializeField]
    private bool useControlRotation = true;
    [SerializeField]
    private float pcSensitivity = 500f;
    [SerializeField]
    private float mobileSensitivity = 50f;
    [SerializeField]
    private CameraController cameraController;

    [Space, Header("Debugging"), Space, SerializeField]
    private bool visibleDebugging = true;
    [SerializeField]
    private Color springArmColor = new Color(0.75f, 0.2f, 0.2f, 0.75f);
    [Range(1f, 10f), SerializeField]
    private float springArmLineWidth = 6f;
    [SerializeField]
    private bool showRaycasts;
    [SerializeField]
    private bool showCollisionProbe;

    private Vector3 endPoint;
    private Vector3 socketPosition;
    private RaycastHit[] hits;
    private Vector3[] raycastPositions;

    private readonly Color collisionProbeColor = new Color(0.2f, 0.75f, 0.2f, 0.15f);

    // Smooth damping
    private Vector3 moveVelocity;
    private Vector3 collisionTestVelocity;

    // Mouse inputs
    private float pitch;
    private float yaw;

    private void Awake()
    {
        Setup();
    }

    private void Start()
    {
        raycastPositions = new Vector3[collisionTestResolution];
        hits = new RaycastHit[collisionTestResolution];
    }

    private void OnValidate()
    {
        raycastPositions = new Vector3[collisionTestResolution];
        hits = new RaycastHit[collisionTestResolution];
    }

    private void Update()
    {
        // If target is null, return from here : Null Reference check
        if (target == null)
        {
            return;
        }

        // Collision check
        if (doCollisionTest)
        {
            CheckCollisions();
        }

        // Set the socketPosition
        SetSocketTransform();

        // Handle mouse inputs for rotations
        if (useControlRotation && Application.isPlaying && cameraController.Controllable)
        {
            Rotate();
        }

        // Follow the target applying targetOffset
        transform.position = Vector3.SmoothDamp(transform.position, target.position + targetOffset, ref moveVelocity, movementSmoothTime);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (!visibleDebugging)
        {
            return;
        }

        // Draw main LineTrace or LineTraces of RaycastPositions, useful for debugging
        Handles.color = springArmColor;

        if (showRaycasts)
        {
            for (int i = 0; i < collisionTestResolution; i++)
            {
                Handles.DrawAAPolyLine(springArmLineWidth, 2, transform.position, raycastPositions[i]);
            }
        }
        else
        {
            Handles.DrawAAPolyLine(springArmLineWidth, 2, transform.position, endPoint);
        }

        // Draw collisionProbe, useful for debugging
        Handles.color = collisionProbeColor;

        if (showCollisionProbe)
        {
            Handles.SphereHandleCap(0, socketPosition, Quaternion.identity, collisionProbeSize * 2f, EventType.Repaint);
        }
    }
#endif

    /// <summary>
    /// Checks for collisions and fill the raycastPositions and hits array.
    /// </summary>
    private void CheckCollisions()
    {
        for (int i = 0, angle = 0; i < collisionTestResolution; i++, angle += 360 / collisionTestResolution)
        {
            Vector3 raycastLocalEndPoint = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0f) * collisionProbeSize;

            raycastPositions[i] = endPoint + transform.rotation * raycastLocalEndPoint;

            Physics.Linecast(transform.position, raycastPositions[i], out hits[i], collisionLayerMask);
        }
    }

    /// <summary>
    /// Sets the translation of children according to filled raycastPositions and hits array data.
    /// </summary>
    private void SetSocketTransform()
    {
        // Offset a point in z direction of targetArmLength by socket offset and translating it into world space.
        Vector3 targetArmOffset = socketOffset - new Vector3(0f, 0f, targetArmLength);

        endPoint = transform.position + transform.rotation * targetArmOffset;

        // If collisionTest is enabled, finds the minDistance
        if (doCollisionTest)
        {
            float minDistance = targetArmLength;

            for (int i = 0; i < collisionTestResolution; i++)
            {
                if (hits[i].collider == null)
                {
                    continue;
                }

                float distance = Vector3.Distance(hits[i].point, transform.position);

                if (minDistance > distance)
                {
                    minDistance = distance;
                }
            }

            // Calculate the direction of children movement
            Vector3 direction = (endPoint - transform.position).normalized;
            Vector3 armOffset = direction * (targetArmLength - minDistance);

            socketPosition = endPoint - armOffset;
        }
        else
        {
            socketPosition = endPoint;
        }

        // Iterate through all children and set their position as socketPosition, using SmoothDamp to smoothly translate the vectors.
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);

            child.position = Vector3.SmoothDamp(child.position, socketPosition, ref collisionTestVelocity, collisionSmoothTime);
        }
    }

    /// <summary>
    /// Handle rotations
    /// </summary>
    private void Rotate()
    {
        float x = cameraController.Input.x;
        float y = cameraController.Input.y;
        float sensitivity = mobileSensitivity;

#if UNITY_EDITOR
        if (x == 0f && y == 0f)
        {
            x = Input.GetAxisRaw("Mouse X");
            y = Input.GetAxisRaw("Mouse Y");
            sensitivity = pcSensitivity;
        }
#endif

        yaw += x * sensitivity * Time.deltaTime;
        pitch -= y * sensitivity * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, -90f, 90f);

        transform.localRotation = Quaternion.Euler(pitch, yaw, 0f);
    }
}
