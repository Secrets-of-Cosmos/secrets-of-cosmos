using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperRobotNoNavMesh : MonoBehaviour
{
    public Transform player;
    public float followDistance = 5.0f;
    public float rayDistance = 10.0f;
    public float proximityDistance = 7.0f;
    public float stopDistance = 7.0f;
    public float rotationSpeed = 1.5f;

    private enum State { Follow, Idle };
    private State state = State.Idle;

    private Animator anim;
    private Rigidbody rb;

    private float idleAnimationTimer = 0.0f;
    private float idleAnimationDuration = 0.5f; // Adjust this duration as needed
    private int currentIdleAnimationIndex = 0;

    private float startFollowDelay = 1.0f; // Adjust this delay as needed
    private float followTimer = 0.0f;

    private string[] idleAnimations = { "GoToRoll_Anim", "RollLoop_Anim", "StopRoll_Anim", "Idle_Anim" };

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {

        float distanceToPlayer = Vector3.Distance(transform.position, new Vector3(player.position.x, transform.position.y, player.position.z));
        switch (state)
        {
            case State.Follow:
                Debug.DrawRay(transform.position, transform.forward * followDistance, Color.red);
                anim.SetBool("GoToRoll_Anim", false);
                anim.SetBool("RollLoop_Anim", false);
                anim.SetBool("StopRoll_Anim", false);
                anim.SetBool("Idle_Anim", false);
                anim.SetBool("Walk_Anim", true);

                if (distanceToPlayer < proximityDistance)
                {
                    state = State.Idle;
                    anim.SetBool("GoToRoll_Anim", false);
                    anim.SetBool("RollLoop_Anim", false);
                    anim.SetBool("StopRoll_Anim", false);
                    anim.SetBool("Idle_Anim", false);
                    anim.SetBool("Walk_Anim", false);
                }
                else
                {
                    if (distanceToPlayer > stopDistance * 3) // Check if the distance is greater than stopDistance * 3
                    {
                       
                        followDistance = 10.0f;
                    }
                    else
                    {
                        followDistance = 4.0f;
                    }
                    // Raycast in the forward direction to detect obstacles
                    if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, rayDistance))
                    {
                        // Calculate a new direction to avoid the obstacle
                        Vector3 avoidDirection = Vector3.Reflect(transform.forward, hit.normal);
                        Vector3 targetPosition = transform.position + avoidDirection * followDistance;
                        Vector3 direction = targetPosition - transform.position;
                        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
                        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                        transform.Translate(Vector3.forward * Time.deltaTime * followDistance);
                    }
                    else
                    {
                        // No obstacle, continue following the player
                        Vector3 targetPosition = player.position + (transform.position - player.position).normalized * followDistance;
                        Vector3 direction = targetPosition - transform.position;
                        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
                        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                        transform.Translate(Vector3.forward * Time.deltaTime * followDistance);
                    }
                }
                break;

            case State.Idle:
                anim.SetBool("Walk_Anim", false);
                idleAnimationTimer += Time.deltaTime;
                if (idleAnimationTimer >= idleAnimationDuration)
                {
                    idleAnimationTimer = 0.1f;
                    PlayIdleAnimations();

                }
                if (distanceToPlayer > proximityDistance)
                {
                    state = State.Follow;
                    anim.SetBool("GoToRoll_Anim", false);
                    anim.SetBool("RollLoop_Anim", false);
                    anim.SetBool("StopRoll_Anim", false);
                    anim.SetBool("Idle_Anim", false);
                    anim.SetBool("Walk_Anim", true);
                }
                break;
        }
    }


    private void PlayIdleAnimations()
    {

        if (currentIdleAnimationIndex == 3)
        {
            anim.SetBool(idleAnimations[0], true);
            anim.SetBool(idleAnimations[1], true);
            anim.SetBool(idleAnimations[2], true);
            anim.SetBool(idleAnimations[3], true);
        }
        else if (currentIdleAnimationIndex == 2)
        {
            anim.SetBool(idleAnimations[0], true);
            anim.SetBool(idleAnimations[1], true);
            anim.SetBool(idleAnimations[2], true);
            anim.SetBool(idleAnimations[3], false);
        }
        else if (currentIdleAnimationIndex == 1)
        {
            anim.SetBool(idleAnimations[0], true);
            anim.SetBool(idleAnimations[1], true);
            anim.SetBool(idleAnimations[2], false);
            anim.SetBool(idleAnimations[3], false);
        }
        else
        {
            anim.SetBool(idleAnimations[0], true);
            anim.SetBool(idleAnimations[1], false);
            anim.SetBool(idleAnimations[2], false);
            anim.SetBool(idleAnimations[3], false);
        }

        currentIdleAnimationIndex = (currentIdleAnimationIndex + 1) % idleAnimations.Length;
    }



}