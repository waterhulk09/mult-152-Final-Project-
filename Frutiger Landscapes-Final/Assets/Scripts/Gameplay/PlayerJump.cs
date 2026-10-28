using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerJump : MonoBehaviour
{
    [Header("Jump Settings")]
    public float jumpHeight = 8f;


    [Header("Input")]
    public InputActionReference jumpAction;

    [Header("is on Ground")]

    public Transform groundCheck;
    public float groundDistance = 0.25f;
    public LayerMask groundLayer;

    [Header("Weight Setting")]
    public PlayerGrab playerGrab;

    private Rigidbody rb;
    private bool isGrounded;
    public float jumpForce = 8f;

public float CurrentJumpForce {get; private set;}
    void Start()
    {   
      rb = GetComponent<Rigidbody>();  
    
    } 
    
    void Update()
    {
        CurrentJumpForce = jumpForce;
        
        if (playerGrab.carriedWeight > 0)
        {
            CurrentJumpForce -= playerGrab.carriedWeight * 0.1f;
        }  
        
          CurrentJumpForce = Mathf.Max(CurrentJumpForce, 2f);


        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);

        if (jumpAction.action.triggered && isGrounded)
        {
            Jump();
        }
    
       void Jump()
    {
        rb.AddForce(Vector3.up * CurrentJumpForce, ForceMode.Impulse);
    }

      }
   }
 

