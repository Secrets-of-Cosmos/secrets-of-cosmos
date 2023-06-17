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
    Vector3 rootMotion;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();;
    }

    private void FixedUpdate()
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");

        animator.SetFloat("Vertical", vertical);
        animator.SetFloat("Horizontal", horizontal);

        OnGround();

        //Vector3 movementDirection = new Vector3(horizontal, 0, vertical);
        //movementDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * movementDirection;
        //movementDirection.Normalize();
        //if (movementDirection != Vector3.zero)
        //{
        //   Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
        //   transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 720f * Time.deltaTime);
        //}

    }

    private void OnAnimatorMove()
    {
        rootMotion += animator.deltaPosition;
    }

    private void OnGround()
    {
        characterController.Move(rootMotion);
        rootMotion = Vector3.zero;
    }
}
