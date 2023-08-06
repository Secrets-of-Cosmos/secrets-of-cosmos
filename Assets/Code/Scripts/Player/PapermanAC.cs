using System.Collections;
using System;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using Cinemachine;

public class PapermanAC : MonoBehaviour
{
    Animator animator;
    AnimatorStateMachine asm;
    CharacterController characterController;
    public Transform cameraTransform;
    public CinemachineVirtualCamera vcam;
    public Animator stateDrivenCamAnimator;

    float vertical;
    float horizontal;
    float turnSmoothVelocity;

    public float jumpSpeed;
    public float jumpHeight;
    public float gravity;

    bool isJumping;
    bool updateJump;
    enum CamType { FirstPerson, ThirdPerson };
    CamType camType;

    Vector3 velocity;
    Vector3 rootMotion;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        var controller = (AnimatorController)animator.runtimeAnimatorController;
        asm = controller.layers[0].stateMachine;

        if (transform.parent && transform.parent.parent && transform.parent.parent.gameObject.name == "interior_iss")
        {

            asm.defaultState = asm.states[4].state; // Changing the default state to Sitting
        }
        else
        {
            asm.defaultState = asm.states[0].state; // Changing the default state the Movement BlendTree
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && characterController.isGrounded)
        {
            updateJump = true;
        }

        //Temporary Sitting to Idle Transition
        if(Input.GetKey(KeyCode.K))
        {
            animator.Play("Sit To Idle");
            asm.defaultState = asm.states[0].state; // Changing the default state the Movement BlendTree
        }
        camType = (CamType)(Convert.ToInt32(stateDrivenCamAnimator.GetBool("isThirdPerson")));
    }

    private void FixedUpdate()
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
        if(camType == CamType.FirstPerson)
        {
            FirstPersonMovement();
        }
        else if(camType == CamType.ThirdPerson)
        {
            ThirdPersonMovement();
        }

        bool movement = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);
        if (movement && Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetFloat("inputMag", 1f, 0.05f, Time.deltaTime);
        }
        else if (movement)
        {
            animator.SetFloat("inputMag", 0.5f, 0.05f, Time.deltaTime);
        }
        else
        {
            animator.SetFloat("inputMag", 0.0f, 0.05f, Time.deltaTime);
        }

        JumpCheck();

    }

    void ThirdPersonMovement()
    {
        if (asm.states[0].state.speed == -1)
            asm.states[0].state.speed = 1;

        Vector3 movementDirection = new Vector3(horizontal, 0, vertical).normalized;
        if (movementDirection != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(movementDirection.x, movementDirection.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 0.1f);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }

    void FirstPersonMovement()
    {
        Vector3 movementDirection = new Vector3(horizontal, 0, vertical);
        if (vcam)
        {
            AnimatorClipInfo[] animInfo = animator.GetCurrentAnimatorClipInfo(0);
            bool isOnGroundMovements = animInfo.Length > 0 && (animInfo[0].clip.name == "Idle" || animInfo[0].clip.name == "Walking");
            if ((Input.GetKey(KeyCode.S)) && isOnGroundMovements)
            {
                asm.states[0].state.speed = -1;
            }
            else if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && isOnGroundMovements)
            {
                asm.states[0].state.speed = 1;
            }

            animInfo = animator.GetCurrentAnimatorClipInfo(0);
            bool isSitting = animInfo.Length > 0 && animInfo[0].clip.name == "Sitting Idle";
            if (movementDirection.x == 0 && movementDirection.z == 0 && !isSitting)
            {
                if(isOnGroundMovements)
                    asm.states[0].state.speed = 1;
                movementDirection = new Vector3(vcam.transform.forward.x, vcam.transform.forward.y, vcam.transform.forward.z * asm.states[0].state.speed);
                if (Vector3.Dot(transform.forward, vcam.transform.forward) >= 0)
                {
                    movementDirection = Vector3.zero;
                }
            }
            else
            {
                movementDirection = movementDirection.z * vcam.transform.forward + vcam.transform.right * movementDirection.x;
            }

            if (isSitting)
            {
                CinemachinePOV cinemachinePov = vcam.GetCinemachineComponent<CinemachinePOV>();
                cinemachinePov.m_HorizontalAxis.Value = Mathf.Clamp
                    (cinemachinePov.m_HorizontalAxis.Value,
                    -90f,
                    90f); // HorizontalAxis value is in range -180f to 180f. So need to clamp to value with half of its value which is -90f to 90f.
            }
        }

        if (movementDirection != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(asm.states[0].state.speed * movementDirection.x, asm.states[0].state.speed * movementDirection.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 0.1f);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }

    private void OnAnimatorMove()
    {
        rootMotion += animator.deltaPosition;
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

        if (isJumping)
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
        if (focus)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
