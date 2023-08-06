using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperRobotNoNavMesh : MonoBehaviour
{
    public Transform player;
    public float proximityDistance = 7.0f;
    public float stopDistance = 7.0f;

    private enum State { Follow, Idle };
    private State state = State.Idle;

    private Animator anim;
    private RobotController robotController;

    private void Start()
    {
        anim = GetComponent<Animator>();
        robotController = GetComponent<RobotController>();
        robotController.enabled = true;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Prevent the Rigidbody from rotating
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, new Vector3(player.position.x, transform.position.y, player.position.z));

        switch (state)
        {
            case State.Follow:
                anim.SetBool("Walk_Anim", true);
                robotController.enabled = true;
                if (distanceToPlayer < proximityDistance)
                {
                    state = State.Idle;
                    anim.SetBool("Walk_Anim", false);
                    anim.SetBool("Jump_Anim", true);
                    robotController.enabled = false;
                }
                break;

            case State.Idle:
                anim.SetBool("Walk_Anim", false);
                anim.SetBool("Jump_Anim", true);
                robotController.enabled = false;
                if (distanceToPlayer > proximityDistance)
                {
                    state = State.Follow;
                    anim.SetBool("Walk_Anim", true);
                    anim.SetBool("Jump_Anim", false);
                    robotController.enabled = true;
                }
                else if (distanceToPlayer <= stopDistance)
                {
                    anim.SetBool("Walk_Anim", false);
                    anim.SetBool("Jump_Anim", true);
                    robotController.enabled = false;
                }
                break;
        }
    }
}
