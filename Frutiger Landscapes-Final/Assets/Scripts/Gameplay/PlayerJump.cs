using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    [Header("Jump Settings")]
    public float jumpHeight = 8f;

    [Header("Input")]
    public InputActionReference jumpAction;

    [Header("Is on Ground")]
    public Transform groundCheck;
    public float groundDistance = 0.25f;
    public LayerMask groundLayer;

    [Header("Weight Setting")]
    public PlayerGrab playerGrab;

    private Rigidbody rb;
    private Animator animator;
    private bool isGrounded;
    public float jumpForce = 8f;

    public float CurrentJumpForce { get; private set; }

    void Start()
    { 
        rb = GetComponent<Rigidbody>();  
        animator = GetComponent<Animator>();
    } 

    void OnEnable()
    {
        jumpAction?.action?.Enable();
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

        // Ground Check
        if (groundCheck != null)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);
        }

        // Update Animator ground parameter
        if (animator != null)
        {
            animator.SetBool("isGrounded", isGrounded);
        }

        // Trigger Jump
        if (jumpAction != null && jumpAction.action != null && jumpAction.action.triggered && isGrounded)
        {
            Jump();
        }
    }

    void Jump()
    {
        // Reset vertical velocity for consistent jump height
        Vector3 vel = rb.linearVelocity; // Change to rb.velocity if on Unity 2022 or older
        vel.y = 0f;
        rb.linearVelocity = vel;

        // Apply impulse
        rb.AddForce(Vector3.up * CurrentJumpForce, ForceMode.Impulse);

        // Trigger Jump Animation
        if (animator != null)
        {
            animator.SetTrigger("Jump");
        }
    }
}
