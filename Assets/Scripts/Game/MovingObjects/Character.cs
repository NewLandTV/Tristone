#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

public class Character : MovingObject
{
    [Header("Movement Settings"), Space, SerializeField]
    private float walkSpeed = 4f;
    [SerializeField]
    private float turnSmoothTime = 0.1f;
    [SerializeField]
    private bool movable = true;
    public bool Movable { get => movable; set => movable = value; }

    [Space, SerializeField]
    private Joystick joystick;

    [Space, Header("Jump Settings"), Space, SerializeField]
    private float jumpForce = 6f;
    [SerializeField]
    private LayerMask groundLayerMask = ~0;
    [SerializeField]
    private Vector3 groundCheckerEndPoint = new Vector3(0f, -0.6f, 0f);

    [Space, Header("Camera Settings"), Space, SerializeField]
    private new Transform camera;
    private float turnSmoothVelocity;

    [Space, Header("UI Settings"), Space, SerializeField]
    private Status status;

    [Space, Header("Debugging"), Space, SerializeField]
    private bool visibleDebugging = true;
    [SerializeField]
    private Color groundCheckerColor = new Color(0.75f, 0.2f, 0.2f, 0.75f);
    [Range(1f, 10f), SerializeField]
    private float groundCheckerLineWidth = 6f;

    private new Rigidbody rigidbody;

    private void Awake()
    {
        Setup();

        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (movable)
        {
            Move();
        }

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
#endif
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (!visibleDebugging)
        {
            return;
        }

        Setup();

        // Draw main LineTrace, useful for debugging
        Handles.color = groundCheckerColor;

        Handles.DrawAAPolyLine(groundCheckerLineWidth, 2, transform.position, transform.position + groundCheckerEndPoint);
    }
#endif

    private void Move()
    {
        float x = joystick.Input.x;
        float z = joystick.Input.y;

        Vector3 direction = new Vector3(x, 0f, z);

#if UNITY_EDITOR
        if (direction == Vector3.zero)
        {
            x = Input.GetAxisRaw("Horizontal");
            z = Input.GetAxisRaw("Vertical");

            direction = new Vector3(x, 0f, z).normalized;
        }
#endif

        if (direction == Vector3.zero)
        {
            return;
        }

        // Move forward and smooth rotation
        float targetAngle = (Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg) + camera.transform.localRotation.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.localRotation.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

        transform.localRotation = Quaternion.Euler(0f, angle, 0f);

        Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

        transform.position += moveDirection.normalized * walkSpeed * Time.deltaTime;
    }

    public void Jump()
    {
        if (rigidbody.velocity.y > 0f)
        {
            return;
        }
        
        if (!Physics.Linecast(transform.position, transform.position + groundCheckerEndPoint, groundLayerMask))
        {
            return;
        }

        rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}
