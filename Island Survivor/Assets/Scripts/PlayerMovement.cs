using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")] [SerializeField] private float moveSpeed = 10f;

    [SerializeField] private float jumpForce = 6f;

    [Header("Ground Check")] [SerializeField]
    private float groundCheckDistance = 1.05f;

    [SerializeField] private LayerMask groundMask = ~0;
    private bool _jumpRequested;

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Horizontal movement input 
        var x = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right
        var z = Input.GetAxisRaw("Vertical"); // W/S or Up/Down


        var horizontal = (transform.right * x + transform.forward * z).normalized * moveSpeed;
        _rb.velocity = new Vector3(horizontal.x, _rb.velocity.y, horizontal.z);

        // Jump input 
        if (Input.GetButtonDown("Jump"))
            _jumpRequested = true;
    }

    private void FixedUpdate()
    {
        if (_jumpRequested && IsGrounded())
        {
            _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        _jumpRequested = false;
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundMask);
    }
}