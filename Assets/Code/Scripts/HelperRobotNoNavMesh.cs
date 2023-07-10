using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperRobotNoNavMesh : MonoBehaviour
{
    public Transform player;
    public float followDistance = 5.0f;
    public float proximityDistance = 7.0f;
    public float stopDistance = 7.0f;
    public float rotationSpeed = 1.5f;

    private enum State { Follow, Idle };
    private State state = State.Idle;

    private Animator anim;
    private Rigidbody rb;
    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Rigidbod'nin dönmesini engelle
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, new Vector3(player.position.x, transform.position.y, player.position.z));
        Debug.Log(distanceToPlayer);
        switch (state)
        {
            case State.Follow:
                anim.SetBool("Walk_Anim", true);
                if (distanceToPlayer < proximityDistance)
                {
                    state = State.Idle;
                    anim.SetBool("Walk_Anim", false);
                }
                else
                {
                    Vector3 targetPosition = player.position + (transform.position - player.position).normalized * followDistance;
                    Vector3 direction = targetPosition - transform.position;
                    Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                    transform.Translate(Vector3.forward * Time.deltaTime * followDistance);
                }
                break;

            case State.Idle:
                anim.SetBool("Walk_Anim", false);
                if (distanceToPlayer > proximityDistance)
                {
                    state = State.Follow;
                    anim.SetBool("Walk_Anim", true);
                }
                else if (distanceToPlayer <= stopDistance)
                {
                    anim.SetBool("Walk_Anim", false);
                }
                break;
        }
    }

}