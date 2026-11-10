using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    [Header("Jump Settings")]
    public float jumpForce = 8f;

    [Header("Input")]
    public InputActionReference jumpAction;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.25f;
    public LayerMask groundLayer;

    [Header("Weight Setting")]
    public PlayerGrab playerGrab;

    private Rigidbody rb;
    private Animator animator;

    private bool isGrounded;

    public float CurrentJumpForce { get; private set; }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>(); // Fixed: Target root animator directly
    }

    void OnEnable()
    {
        jumpAction?.action?.Enable(); // Required for Input System to listen
    }

    void OnDisable()
    {
        jumpAction?.action?.Disable();
    }

    void Update()
    {
        CurrentJumpForce = jumpForce;

        if (playerGrab != null && playerGrab.carriedWeight > 0)
        {
            CurrentJumpForce -= playerGrab.carriedWeight * 0.1f;
        }

        CurrentJumpForce = Mathf.Max(CurrentJumpForce, 2f);

        // Safety check if groundCheck transform is assigned
        if (groundCheck != null)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);
        }

        if (animator != null)
        {
            animator.SetBool("isGrounded", isGrounded);
        }

        if (jumpAction != null && jumpAction.action != null && jumpAction.action.triggered && isGrounded)
        {
            Jump();
        }
    }

    void Jump()
    {
        // Optional: Zero out vertical velocity so jumps feel consistent regardless of falling momentum
        Vector3 vel = rb.linearVelocity;
        vel.y = 0f;
        rb.linearVelocity = vel;

        rb.AddForce(Vector3.up * CurrentJumpForce, ForceMode.Impulse);

        if (animator != null)
        {
            animator.SetTrigger("Jump");
        }
    }
}