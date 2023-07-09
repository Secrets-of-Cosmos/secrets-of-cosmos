using UnityEngine;
using UnityEngine.AI;

public class HelperRobot : MonoBehaviour
{
    public Transform player;
    public float followDistance = 3.0f;
    public float proximityDistance = 2.0f;
    public float walkAroundDistance = 5.0f;
    public float stopDistance = 1.5f;

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
        float distanceToPlayer = Vector3.Distance(transform.position, new Vector3(player.position.x, transform.position.y, player.position.z));

        switch (state)
        {
            case State.Follow:
                anim.SetBool("Walk_Anim", true);
                if (distanceToPlayer < proximityDistance)
                {
                    state = State.Idle;
                    timeSinceStateChanged = Time.time;
                    agent.isStopped = true; // Robot durduğunda NavMeshAgent'i durdur
                }
                break;
            case State.Idle:
                anim.SetBool("Walk_Anim", false);
                if (distanceToPlayer > proximityDistance)
                {
                    state = State.Follow;
                    timeSinceStateChanged = Time.time;
                    agent.isStopped = false; // Robot hareket etmeye başladığında NavMeshAgent'i çalıştır
                }
                else if (distanceToPlayer <= stopDistance)
                {
                    agent.isStopped = true; // Robot oyuncuya çok yaklaştığında NavMeshAgent'i durdur
                }
                break;
        }

        agent.destination = player.position;
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
