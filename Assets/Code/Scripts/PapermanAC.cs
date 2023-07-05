using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PapermanAC : MonoBehaviour
{
    Animator animator;
    CharacterController characterController;
    public Transform cameraTransform;


    float vertical;
    float horizontal;
    float turnSmoothVelocity;

    public float jumpSpeed;
    public float jumpHeight;
    public float gravity;

    bool isJumping;
    bool updateJump;

    Vector3 velocity;
    Vector3 rootMotion;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space) && characterController.isGrounded)
        {
            updateJump = true;
        }
    }
    private void FixedUpdate()
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
        Vector3 movementDirection = new Vector3(horizontal, 0, vertical).normalized;
        if(movementDirection != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(movementDirection.x, movementDirection.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 0.1f);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }


        bool movement = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) 
            || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

        if (movement && Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetFloat("inputMag", 1f, 0.05f, Time.deltaTime);
        }
        else if(movement)
        {
            animator.SetFloat("inputMag", 0.5f, 0.05f, Time.deltaTime);
        }
        else
        {
            animator.SetFloat("inputMag", 0.0f, 0.05f, Time.deltaTime);
        }

        JumpCheck();

    }

    private void OnAnimatorMove()
    {
        if(characterController.isGrounded)
        {
            rootMotion += animator.deltaPosition;
        }
    }

    private void Jump()
    {
        isJumping = true;
        velocity = animator.velocity * jumpSpeed;
        velocity.y = Mathf.Sqrt(2 * gravity * jumpHeight);
    }

    private void JumpCheck()
    {
        if (updateJump)
        {
            animator.SetBool("isJumpingUp", true);
            Jump();
            updateJump = false;
        }
        else
        {
            animator.SetBool("isJumpingUp", false);
        }

        if (isJumping | !characterController.isGrounded)
        {
            animator.SetBool("isFalling", true);
            velocity.y -= gravity * Time.deltaTime;
            characterController.Move(velocity * Time.deltaTime);
            isJumping = !characterController.isGrounded;
            rootMotion = Vector3.zero;
        }
        else
        {
            characterController.Move(rootMotion + Vector3.down);
            rootMotion = Vector3.zero;
        }

        if (characterController.isGrounded)
        {
            animator.SetBool("isFalling", false);
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if(focus)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
