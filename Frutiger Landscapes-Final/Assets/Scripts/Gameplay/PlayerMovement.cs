using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float rotationSpeed = 15f;

    [Header("Input Actions")]
    public InputActionReference moveAction;
    public InputActionReference sprintAction;

    private Rigidbody rb;
    private Vector2 moveInput;
    private bool isSprinting;
    private Transform camTransform;

    private Animator animator;
    [SerializeField] private float animationDampTime = 0.1f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        if (Camera.main != null)
        {
            camTransform = Camera.main.transform;
        }
    }

    void OnEnable()
    {
        moveAction?.action?.Enable();
        sprintAction?.action?.Enable();
    }

    void OnDisable()
    {
        moveAction?.action?.Disable();
        sprintAction?.action?.Disable();
    }

    void Update()
    {
        // 1. Read Inputs
        if (moveAction != null && moveAction.action != null)
        {
            moveInput = moveAction.action.ReadValue<Vector2>();
        }

        if (sprintAction != null && sprintAction.action != null)
        {
            isSprinting = sprintAction.action.IsPressed();
        }

        // 2. Update Animator Parameters
        if (animator != null)
        {
            float inputMagnitude = moveInput.magnitude;
            float targetAnimationSpeed = 0f;

            if (inputMagnitude > 0.05f)
            {
                targetAnimationSpeed = isSprinting ? 1.0f : 0.5f;
            }

            // Uses animationDampTime to clear the warning and smooth out transitions
            animator.SetFloat("Speed", targetAnimationSpeed, animationDampTime, Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        // 3. Camera-Relative Direction
        Vector3 forward = camTransform != null ? camTransform.forward : Vector3.forward;
        Vector3 right = camTransform != null ? camTransform.right : Vector3.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = (forward * moveInput.y + right * moveInput.x).normalized;
        float currentSpeed = isSprinting ? runSpeed : walkSpeed;

        // 4. Apply Rigidbody Velocity
        Vector3 targetVelocity = moveDirection * currentSpeed;
        targetVelocity.y = rb.linearVelocity.y; // Change to rb.velocity.y if using Unity 2022 or older
        rb.linearVelocity = targetVelocity;

        // 5. Rotate Character to Facing Direction
        if (moveInput.magnitude > 0.05f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
        }
    }
}