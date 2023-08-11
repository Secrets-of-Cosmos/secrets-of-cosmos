using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperRobotNoNavMesh : MonoBehaviour
{
    public GameObject target;
    private Transform player;
    private Vector3[] rayArray;
    private Vector3 lerpedTargetDir;
    public float rayLength = 2;
    public float lerpSpeed = 1.0f;
    public float mult = 1.5f;
    public float speed = 1f;
    public float rotSpeed = 0.15f;
    public float stopDistance = 4f;
    public float proximityDistance = 4f;


    private enum State { Follow, Idle };
    private State state = State.Idle;

    private Animator anim;
    private Rigidbody rb;

    private float idleAnimationTimer = 0.0f;
    private float idleAnimationDuration = 0.5f; // Adjust this duration as needed
    private int currentIdleAnimationIndex = 0;

    private string[] idleAnimations = { "GoToRoll_Anim", "RollLoop_Anim", "StopRoll_Anim", "Idle_Anim" };

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        player = target.transform;
        rayArray = new Vector3[3];

    }

    private void Update()
    {
        player = target.transform;
        float distanceToPlayer = Vector3.Distance(transform.position, new Vector3(player.position.x, transform.position.y, player.position.z));
        switch (state)
        {
            case State.Follow:
                Debug.Log(distanceToPlayer);
                anim.SetBool("GoToRoll_Anim", false);
                anim.SetBool("RollLoop_Anim", false);
                anim.SetBool("StopRoll_Anim", false);
                anim.SetBool("Idle_Anim", false);
                anim.SetBool("Walk_Anim", true);
                if (distanceToPlayer > 1000f)
                {
                    anim.SetBool("GoToRoll_Anim", false);
                    anim.SetBool("RollLoop_Anim", false);
                    anim.SetBool("StopRoll_Anim", false);
                    anim.SetBool("Idle_Anim", false);
                    anim.SetBool("Walk_Anim", true);
                    Vector3 teleportPosition = new Vector3(player.position.x, player.position.y + 4, player.position.z - 6);
                    transform.position = teleportPosition;
                }
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
                    if (distanceToPlayer > stopDistance * 3 && distanceToPlayer < 30f)
                    {

                        speed = 4.0f;
                    }
                    else
                    {
                        speed = 2f;
                    }
                    MoveTowardsAndAvoid(target.transform);

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
    private void MoveTowardsAndAvoid(Transform target)
    {

        Vector3 targetPos = target.position;
        targetPos.y = transform.position.y;
        Vector3 targetDir = targetPos - transform.position;

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;
        Vector3 up = transform.up;

        rayArray[0] = transform.forward + (transform.right * -0.50f) + (transform.up * 0.2f);
        rayArray[1] = transform.forward + (transform.up * 0.50f);
        rayArray[2] = transform.forward + (transform.right * 0.50f) + (transform.up * 0.2f);



        bool moveIt = false;

        for (int i = 0; i < 3; i++)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, rayArray[i], out hit, rayLength))
            {
                Quaternion newRotation = Quaternion.LookRotation(Vector3.Reflect(targetDir.normalized, hit.normal));
                transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, rotSpeed * Time.deltaTime);
                targetDir += mult * hit.normal;

            }
            else
            {
                moveIt = true;
            }
           Debug.DrawLine(transform.position, rayArray[i], Color.magenta);
           
        }
       
        lerpedTargetDir = Vector3.Lerp(lerpedTargetDir, targetDir, Time.deltaTime * lerpSpeed);
        Debug.DrawRay(transform.position, targetDir, Color.blue);
        Debug.DrawRay(transform.position, lerpedTargetDir, Color.green);
        Quaternion targetRotation = Quaternion.LookRotation(lerpedTargetDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);

        if (moveIt)
        {
            transform.Translate(targetDir.normalized* Time.deltaTime * speed);
        }
    }



}