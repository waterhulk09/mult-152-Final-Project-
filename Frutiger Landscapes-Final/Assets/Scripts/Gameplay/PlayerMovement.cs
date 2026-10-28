using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;

    [Header("Input Actions")]
    public InputActionReference moveAction;
    public InputActionReference sprintAction; // Drag your Sprint input action here (e.g. Left Shift / Controller Button)

    private Rigidbody rb;
    private Vector2 moveInput;
    private bool isSprinting;
    private Transform camTransform;

    // --- Animator Integration ---
    private Animator animator;
    [SerializeField] private float animationDampTime = 0.1f; // Smooths out parameter changes

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
        if (moveAction != null && moveAction.action != null)
        {
            moveInput = moveAction.action.ReadValue<Vector2>();
        }

        // Read sprint input (returns true if holding Sprint key)
        if (sprintAction != null && sprintAction.action != null)
        {
            isSprinting = sprintAction.action.IsPressed();
        }

        if (animator != null)
        {
            float inputMagnitude = moveInput.magnitude;
            float targetAnimationSpeed = 0f;

            if (inputMagnitude > 0.05f)
            {
              
                targetAnimationSpeed = isSprinting ? 1.0f : 0.5f;
            }
            else
            {
        
                targetAnimationSpeed = 0f;
            }

        
            animator.SetFloat("Speed", targetAnimationSpeed, animationDampTime, Time.deltaTime);
        }
    }
}