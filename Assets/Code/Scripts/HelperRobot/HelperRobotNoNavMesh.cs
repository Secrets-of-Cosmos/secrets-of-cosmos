using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperRobotNoNavMesh : MonoBehaviour
{
    public GameObject target;
    private Transform player;
    private Vector3 lerpedTargetDir;
    public float rayLength = 5;
    public float speed = 1.5f;
    public float rotSpeed = 0.15f;
    public float stopDistance = 4f;
    public float proximityDistance = 4f;
    // Define the number of directions to consider for ray casting
    private const int NumDirections = 8;

    // Define the maximum distance for ray casting
    private const float MaxDistance = 2.0f;

    // Define the borrowing factors for neighbors
    private const float BorrowNeighbor1 = 0.5f;
    private const float BorrowNeighbor2 = 0.25f;

    private enum State { Follow, Idle };
    private State state = State.Idle;

    private Animator anim;
    private Rigidbody rb;

    private float idleAnimationTimer = 0.0f;
    private float idleAnimationDuration = 0.5f;
    private int currentIdleAnimationIndex = 0;

    private string[] idleAnimations = { "GoToRoll_Anim", "RollLoop_Anim", "StopRoll_Anim", "Idle_Anim" };

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        player = target.transform;
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
                    Vector3 teleportPosition = new Vector3(player.position.x, player.position.y + 3, player.position.z);
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

                        MoveTowardsAndAvoid(target.transform, speed*2);
                    }
                    else
                    {
                        MoveTowardsAndAvoid(target.transform, speed);
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
    private void MoveTowardsAndAvoid(Transform target, float speed)
    {
        Vector3 moveDirection = (target.position - transform.position).normalized;
        List<Vector3> directions = CalculateDirections();

        float maxPriority = float.MinValue;
        Vector3 bestDirection = Vector3.zero;

        foreach (Vector3 direction in directions)
        {
            float priority = CalculatePriority(direction, moveDirection);
            Debug.DrawRay(transform.position, direction );
            if (priority > maxPriority)
            {
                maxPriority = priority;
                bestDirection = direction;
            }
        }

        // Move the robot towards the best direction while avoiding obstacles
        Vector3 newPosition = transform.position + (bestDirection * Time.deltaTime*speed);
        transform.position = newPosition;
    }

    private List<Vector3> CalculateDirections()
    {
        List<Vector3> directions = new List<Vector3>();
        float angleIncrement = 360.0f / NumDirections;

        for (int i = 0; i < NumDirections; i++)
        {
            float angle = i * angleIncrement;
            Vector3 direction = Quaternion.Euler(0, angle, 0) * transform.forward;
            directions.Add(direction);
        }

        return directions;
    }

    private float CalculatePriority(Vector3 direction, Vector3 moveDirection)
    {
        float dotProduct = Vector3.Dot(direction, moveDirection);

        // Calculate the collision factor
        RaycastHit hit;
        bool hasHit = Physics.Raycast(transform.position, direction, out hit, MaxDistance);
        float collisionFactor = hasHit ? (1.0f - hit.distance / MaxDistance) : 1.0f;

        // Calculate final priority
        float priority = dotProduct * collisionFactor;
        float borrowedPriority = (1.0f - collisionFactor) * BorrowNeighbor1 +
                                (1.0f - BorrowNeighbor1) * BorrowNeighbor2;
        priority += borrowedPriority;

        return priority;
    }



}