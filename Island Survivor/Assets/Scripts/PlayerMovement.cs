using UnityEngine;

[RequireComponent(typeof(Rigidbody),typeof(Animator))]
public class PlayerMovement : MonoBehaviour {
    [Header("Movement")] [SerializeField] float moveSpeed = 10f; 
    [SerializeField] float jumpForce = 6f;
    [Header("Ground Check")] [SerializeField] float groundCheckDistance = 1.05f;
    [SerializeField] LayerMask groundMask = ~0;

    Rigidbody _rb;
    Animator _anim;
    bool _jumpRequested;

    void Awake() {
        _rb   = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
    }

    void Update() {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 dir = (transform.right*x + transform.forward*z).normalized;
        _rb.velocity = new Vector3(dir.x*moveSpeed, _rb.velocity.y, dir.z*moveSpeed);

        // feed Speed param for Idle<->Run
        _anim.SetFloat("Speed", new Vector2(x,z).magnitude);

        if (Input.GetButtonDown("Jump"))
            _jumpRequested = true;
            _anim.SetTrigger("Jump");
    }

    void FixedUpdate() {
        bool grounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundMask);

        // feed ground & vertical velocity for your Jump transitions
        _anim.SetBool("isGrounded", grounded);
        _anim.SetFloat("yVelocity", _rb.velocity.y);

        if (_jumpRequested && grounded) {
            _rb.velocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            _anim.SetTrigger("Jump");
        }

        _jumpRequested = false;
    }
}