using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MoveTo : MonoBehaviour
{
    public Transform player;
    public float followDistance = 3.0f;
    public float proximityDistance = 3.0f;
    public float walkAroundDistance = 5.0f;
    public float stopDistance = 1.5f;

    private enum State { Follow, Idle, WalkAround, WalkRandom };
    private State state = State.Idle;

    private Animator anim;
    private float timeSinceStateChanged;
    // Start is called before the first frame update
    NavMeshAgent navMeshAgent;
    void Start()

    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        timeSinceStateChanged = Time.time;


    }

    // Update is called once per frame
    void Update()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent != null)
        {
            navMeshAgent.destination = new Vector3(player.position.x, transform.position.y, player.position.z);
            timeSinceStateChanged = Time.time;

        }



    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(player.position, followDistance);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, proximityDistance);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, stopDistance);

        Gizmos.color = Color.yellow;
        Debug.Log(navMeshAgent.destination);
        Gizmos.DrawWireSphere(navMeshAgent.destination, 3f);
    }
}