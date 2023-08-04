using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    public float maxSpeed;
    public float maxDistance = 10f;
    public float openDelay = 0.5f;

    private float speed = 0f;
    private Animator anim;
    private bool canMove = false;
    private bool movingToFront = false;

    public AudioClip footStepSound;
    private AudioSource audioSource;
    public bool soundActivated = false;

    public float circleRadius = 10f;
    public float circleSpeed = 2f;
    private float angle = 0f;
    private enum RobotState { Circling, Idle, MovingBackAndForth }
    private RobotState currentState;
    private float stateChangeTimer;
    private Vector3 movingDirection;

    private void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        if (anim.GetBool("Open_Anim"))
        {
            StartCoroutine(WaitCoroutine(openDelay, () => canMove = true));
        }
        ChangeState(RobotState.Idle);
    }

    private void Update()
    {
        if (!movingToFront) 
        {
            HandleMovement();
        }
        HandlePlayerInteraction();
        HandleRobotStates();
        HandleSound();
    }

    private void HandleMovement()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance > maxDistance && canMove)
        {
            anim.SetBool("Walk_Anim", true);
            speed = Mathf.Lerp(0, maxSpeed, (distance - maxDistance) / maxDistance);
            Vector3 playerDirection = player.position - transform.position;
            playerDirection.y = 0.0f;
            transform.rotation = Quaternion.LookRotation(playerDirection);
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
    }

    private void HandlePlayerInteraction()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            movingToFront = !movingToFront;
            anim.SetBool("Walk_Anim", anim.GetBool("Walk_Anim") ? false : true);
        }
        if (movingToFront)
        {
            Vector3 destination = player.position + player.forward * 5;
            destination.y = transform.position.y;
            float distance = Vector3.Distance(destination, transform.position);
            if (distance < 0.1f)
            {
                anim.SetBool("Walk_Anim", false);
                transform.LookAt(player.position);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * speed);
                anim.SetBool("Walk_Anim", true);
            }
        }
    }

    private void HandleRobotStates()
    {
        stateChangeTimer -= Time.deltaTime;
        if (stateChangeTimer <= 0)
        {
            ChangeState((RobotState)Random.Range(0, 3));
        }

        switch (currentState)
        {
            case RobotState.Circling:
                CircularMovement();
                break;
            case RobotState.Idle:
                anim.SetBool("Walk_Anim", false);
                break;
            case RobotState.MovingBackAndForth:
                MoveBackAndForth();
                break;
        }
    }

    private void ChangeState(RobotState newState)
    {
        currentState = newState;
        stateChangeTimer = Random.Range(1f, 2f);
        if (currentState == RobotState.MovingBackAndForth)
        {
            movingDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
            speed = Random.Range(1f, maxSpeed);
        }
    }

    private void CircularMovement()
    {
        anim.SetBool("Walk_Anim", true);
        angle += circleSpeed * Time.deltaTime;
        Vector3 offset = new Vector3(Mathf.Sin(angle), 0f, Mathf.Cos(angle)) * circleRadius;
        Vector3 targetPosition = new Vector3(player.position.x + offset.x, transform.position.y, player.position.z + offset.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * circleSpeed);
        transform.LookAt(targetPosition);
    }

    private void MoveBackAndForth()
    {
        anim.SetBool("Walk_Anim", true);
        transform.LookAt(transform.position + movingDirection);
        Debug.Log(speed);
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    private void HandleSound()
    {
        if (anim.GetBool("Walk_Anim") && !audioSource.isPlaying && soundActivated)
        {
            audioSource.PlayOneShot(footStepSound);
        }
    }

    private IEnumerator WaitCoroutine(float time, System.Action onComplete)
    {
        yield return new WaitForSeconds(time);
        onComplete();
    }
}
