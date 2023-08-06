using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerRover : MonoBehaviour
{
    public float motorTorque = 200f;
    public float maxSteerAngle = 30f;
    public bool playerControlling = false;
    public Transform armVertical;
    public Transform armHorizontal;
    public SphereCollider armCollider;
    public WheelCollider[] frontWheelColliders;
    public WheelCollider[] rearWheelColliders;
    LayerMask collectableLayer;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        collectableLayer = LayerMask.NameToLayer("Collectable");

    }

    // Update is called once per frame
    void Update()
    {
        if (playerControlling)
        {
            InputControl();
            CollisionCheck();
        }
    }

    void FixedUpdate()
    {
        if (playerControlling)
        {
            ArmControl();
            Move();
        }
    }

    void CollisionCheck()
    {

        Collider[] hitColliders = Physics.OverlapSphere(armCollider.bounds.center, armCollider.radius, 1 << collectableLayer);
        if (hitColliders.Length > 0)
        {
            hitColliders[0].GetComponent<Collectable>().Collect(armCollider.transform);
        }
    }

    private void Move()
    {
        float motorForce = motorTorque * Input.GetAxis("Vertical");
        float steerAngle = maxSteerAngle * Input.GetAxis("Horizontal");

        for (int i = 0; i < frontWheelColliders.Length; i++)
        {
            frontWheelColliders[i].steerAngle = steerAngle;
        }

        for (int i = 0; i < rearWheelColliders.Length; i++)
        {
            rearWheelColliders[i].motorTorque = motorForce;
        }
    }

    private void ArmControl()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            armVertical.Rotate(0, 1, 0, Space.World);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            armVertical.Rotate(0, -1, 0, Space.World);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            armHorizontal.Rotate(0, 1, 0, Space.Self);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            armHorizontal.Rotate(0, -1, 0, Space.Self);
        }

    }

    void InputControl()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DropItem();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(armCollider.bounds.center, armCollider.radius);
    }

    private void DropItem()
    {
        Collectable collectable = armCollider.GetComponentInChildren<Collectable>();
        if (collectable != null)
        {
            collectable.Drop();
        }
    }
}
