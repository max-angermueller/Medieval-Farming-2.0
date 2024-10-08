using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.WSA;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform playerCamera = null;
    [SerializeField] float mouseSensitivity = 3.5f;
    [SerializeField] float gravity = -10f;
    [SerializeField] float walkSpeed = 6.0f;
    [SerializeField] float runSpeed = 12.0f;
    [SerializeField] float jumpHeight = 1.5f;

    float cameraPitch = 0.0f;
    float velocityY = 0.0f;
    [SerializeField] bool lockCursor = true;

    CharacterController controller = null;

    [SerializeField][Range(0.0f, 0.5f)] float moveSmoothTime = 0.3f;
    [SerializeField][Range(0.0f, 0.5f)] float mouseSmoothTime = 0.03f;

    

    Vector2 currentDir = Vector2.zero;
    Vector2 currentDirVelocity = Vector2.zero;

    Vector2 currentMouseDelta = Vector2.zero;
    Vector2 currentMouseDeltaVelocity = Vector2.zero;

    [SerializeField] private LayerMask groundMask;  // Layer, der den Boden repräsentiert
    [SerializeField] private float groundCheckDistance = 0.1f;  // Distanz für den Boden-Check
    
   


    void Start()
    {
        controller = GetComponent<CharacterController>();
        

        if (lockCursor)
        {
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            //UnityEngine.Cursor.visible = false;
        }
    }
       

    void Update()
    {
        UpdateMouseLook();
        UpdateMovement();
    }

    void UpdateMouseLook()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);
        //Vertical rotation (Nicken)
        cameraPitch -= currentMouseDelta.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);
        playerCamera.localEulerAngles = Vector3.right * cameraPitch;

        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
    }
    void UpdateMovement()
    {
        //Get Input from keyboard for direction of movement (W, A, S, D)
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();
        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

        // Bewegungsgeschwindigkeit festlegen(rennen oder gehen)
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundMask);
        //Debug.DrawRay(transform.position, Vector3.down, Color.red, 20.0f);
        
        if (isGrounded) {
            
            velocityY = 0.0f;
            
            if (Input.GetButtonDown("Jump"))  // Standardmäßige Leertaste für Sprung
            {
                velocityY = Mathf.Sqrt(jumpHeight * -2f * gravity);  // Sprungkraft basierend auf der Höhe
            }
        }

        // apply gravity
        velocityY += gravity * Time.deltaTime;

        //Direction & movement speed
        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * currentSpeed + Vector3.up * velocityY;

        controller.Move(velocity * Time.deltaTime);
    }
}

