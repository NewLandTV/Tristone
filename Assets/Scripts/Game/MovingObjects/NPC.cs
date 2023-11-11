#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

public class NPC : MovingObject
{
    [Header("Movement Settings"), Space, SerializeField]
    private float walkSpeed = 4f;
    [SerializeField]
    private float turnSmoothTime = 0.1f;
    private Vector3 direction;

    [Space, Header("Jump Settings"), Space, SerializeField]
    private float jumpForce = 6f;
    [SerializeField]
    private LayerMask groundLayerMask = ~0;
    [SerializeField]
    private Vector3 groundCheckerEndPoint = new Vector3(0f, -0.6f, 0f);

    [Space, Header("Debugging"), Space, SerializeField]
    private bool visibleDebugging = true;
    [SerializeField]
    private Color groundCheckerColor = new Color(0.75f, 0.2f, 0.2f, 0.75f);
    [Range(1f, 10f), SerializeField]
    private float groundCheckerLineWidth = 6f;

    private float currentAngle;
    private float turnSmoothVelocity;

    private new Rigidbody rigidbody;

    private void Awake()
    {
        Setup();

        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Move();

        if (Random.Range(0, 100) == 0)
        {
            int x = Random.Range(-1, 2);
            int z = Random.Range(-1, 2);

            direction = new Vector3(x, 0f, z).normalized;
        }

        if (Random.Range(0, 25) == 0)
        {
            currentAngle = Random.Range(0f, 360f);
        }
        else if (Random.Range(0, 1) == 0)
        {
            currentAngle += Time.deltaTime;
        }
        else if (Random.Range(0, 1) == 0)
        {
            currentAngle -= Time.deltaTime;
        }

        if (Random.Range(0, 250) == 0)
        {
            Jump();
        }
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
        if (direction == Vector3.zero)
        {
            return;
        }

        // Move forward and smooth rotation
        float targetAngle = (Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg) + currentAngle;
        float angle = Mathf.SmoothDampAngle(transform.localRotation.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

        transform.localRotation = Quaternion.Euler(0f, angle, 0f);

        Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

        transform.position += moveDirection.normalized * walkSpeed * Time.deltaTime;
    }

    private void Jump()
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
