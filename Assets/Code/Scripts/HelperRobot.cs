using UnityEngine;
using UnityEngine.AI;

public class HelperRobot : MonoBehaviour
{
    public Transform player;
    public float followDistance = 3.0f;
    public float proximityDistance = 2.0f;
    public float walkAroundDistance = 5.0f;

    private enum State { Follow, Idle, WalkAround, WalkRandom };
    private State state = State.Idle;

    private Animator anim;
    private NavMeshAgent agent;
    private float timeSinceStateChanged;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        timeSinceStateChanged = Time.time;
        agent.SetDestination(new Vector3(0, 0, 0));
    }

    private void Update()
    {
        switch (state)
        {
            case State.Follow:
                anim.SetBool("Walk_Anim", true);
                if (Vector3.Distance(transform.position, new Vector3(agent.destination.x, transform.position.y, agent.destination.z)) < proximityDistance)
                {
                    state = State.Idle;
                    timeSinceStateChanged = Time.time;
                }
                break;
            case State.Idle: 
                anim.SetBool("Walk_Anim", false);
                agent.isStopped = true;
                if (Vector3.Distance(transform.position, new Vector3(player.position.x, transform.position.y, player.position.z)) > proximityDistance)
                {
                    state = State.Follow;
                    timeSinceStateChanged = Time.time;
                    agent.SetDestination(new Vector3(10, 0, 10));
                    // agent.SetDestination(new Vector3(player.position.x, transform.position.y, player.position.z));
                }
                break;
            // case State.Idle:
            //     anim.SetBool("Walk_Anim", false);
            //     agent.isStopped = true;
            //     if (Time.time - timeSinceStateChanged > 3.0f)
            //     {
            //         state = Random.Range(0, 2) == 0 ? State.WalkAround : State.WalkRandom;
            //         timeSinceStateChanged = Time.time;
            //         Vector3 randomPos = transform.position + Random.onUnitSphere * 5.0f;
            //         agent.SetDestination(randomPos);
            //         agent.isStopped = false;
            //     }
            //     break;

            // case State.WalkAround:
            //     anim.SetBool("Walk_Anim", true);
            //     if (Time.time - timeSinceStateChanged > 3.0f)
            //     {
            //         state = State.Follow;
            //         timeSinceStateChanged = Time.time;
            //     }
            //     break;

            // case State.WalkRandom:
            //     anim.SetBool("Walk_Anim", true);
            //     if (Time.time - timeSinceStateChanged > 5.0f)
            //     {
            //         state = State.Follow;
            //         timeSinceStateChanged = Time.time;
            //     }
            //     break;
        }

        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     state = State.Follow;
        //     Vector3 inFrontOfPlayer = player.position + player.forward * -5;
        //     agent.SetDestination(inFrontOfPlayer);
        //     transform.LookAt(player.position);
        // }
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(player.position, followDistance);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, proximityDistance);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, walkAroundDistance);

        Gizmos.color = Color.yellow;
        Debug.Log(agent.destination);
        Gizmos.DrawWireSphere(agent.destination, 3f);
    }
}
