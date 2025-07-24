using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")] [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float jumpForce = 6f;
    [Header("Ground Check")] [SerializeField] private float groundCheckDistance = 1.05f;
    [SerializeField] private LayerMask groundMask = ~0;
    private Rigidbody _rb;
    private Animator _anim;
    private bool _jumpRequested;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        Vector3 dir = (transform.right * x + transform.forward * z).normalized;
        _rb.velocity = new Vector3(dir.x * moveSpeed, _rb.velocity.y, dir.z * moveSpeed);
        _anim.SetFloat("MoveZ", z);
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            _jumpRequested = true;
            _anim.SetTrigger("Jump");
        }
    }

    private void FixedUpdate()
    {
        if (_jumpRequested)
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