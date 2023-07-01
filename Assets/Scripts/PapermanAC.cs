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
        animator.SetBool("isMoving", true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && characterController.isGrounded)
        {
            updateJump = true;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && characterController.isGrounded && Input.GetKey("w"))
        {
            bool run = animator.GetBool("isRunning");
            animator.SetBool("isRunning", !run);
        }
        else if (!Input.GetKey("w") | Input.GetKeyUp(KeyCode.LeftShift))
        {
            animator.SetBool("isRunning", false);
        }
    }
    private void FixedUpdate()
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
        //Vector3 movementDirection = new Vector3(horizontal, 0, vertical).normalized;
        //if (movementDirection != Vector3.zero)
        //{
        //    float targetAngle = Mathf.Atan2(movementDirection.x, movementDirection.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
        //    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 0.1f);
        //    transform.rotation = Quaternion.Euler(0f, angle, 0f);
        //}
        animator.SetFloat("Vertical", vertical, 0.1f, Time.deltaTime);
        animator.SetFloat("Horizontal", horizontal, 0.1f, Time.deltaTime);
        //bool movement = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) 
        //    || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);
        //if (movement && Input.GetKey(KeyCode.LeftShift))
        //{
        //    animator.SetFloat("inputMag", 1f, 0.05f, Time.deltaTime);
        //}
        //else if(movement)
        //{
        //    animator.SetFloat("inputMag", 0.5f, 0.05f, Time.deltaTime);
        //}
        //else
        //{
        //    animator.SetFloat("inputMag", 0.0f, 0.05f, Time.deltaTime);
        //}


        //Vector3 movementDirection = new Vector3(horizontal, 0, vertical);
        //movementDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * movementDirection;
        //movementDirection.Normalize();
        //if (movementDirection != Vector3.zero && Input.GetKey("w"))
        //{
        //    Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
        //    transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 720f * Time.deltaTime);
        //}




        if (updateJump)
        {
            animator.SetBool("isJumpingUp", true);
            animator.SetBool("isMoving", false);
            Jump();
            updateJump = false;
        }
        else
        {
            animator.SetBool("isJumpingUp", false);
        }

        if(isJumping)
        {
            animator.SetBool("isFalling", true);
            animator.SetBool("isMoving", false);
            velocity.y -= gravity * Time.deltaTime;
            characterController.Move(velocity * Time.deltaTime);
            isJumping = !characterController.isGrounded;
            rootMotion = Vector3.zero;
        }
        else
        {
            characterController.Move(rootMotion + Vector3.down);
            rootMotion = Vector3.zero;
            animator.SetBool("isMoving", true);
        }

        if(characterController.isGrounded)
        {
            animator.SetBool("isFalling", false);
        }
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
