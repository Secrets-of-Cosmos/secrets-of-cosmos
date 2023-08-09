using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerRover : MonoBehaviour
{
    public Transform armVertical;
    public Transform armHorizontal;
    public SphereCollider armCollider;
    public Camera roverCamera;
    LayerMask collectableLayer;
    private Rigidbody rb;
    private PapermanAC player;
    public enum ControlMode { Player, Random }
    public ControlMode controlMode = ControlMode.Player;
    public float motorForce = 200f;
    public float maxSteerAngle = 30f;
    public float randomDirectionChangeTime = 3f; // Rastgele yön değişikliği süresi

    public WheelCollider[] wheelColliders;

    private float h, v;
    private float timeSinceLastDirectionChange;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<CollectMission>().player;
        rb = GetComponent<Rigidbody>();
        collectableLayer = LayerMask.NameToLayer("Collectable");

    }

    // Update is called once per frame
    void Update()
    {
        if (controlMode == ControlMode.Player)
        {
            InputControl();
            CollisionCheck();
            ArmControl();
        }
    }

    void FixedUpdate()
    {
        switch (controlMode)
        {
            case ControlMode.Player:
                h = Input.GetAxis("Horizontal");
                v = Input.GetAxis("Vertical");

                break;

            case ControlMode.Random:
                v = 0;
                h = 0;
                if (timeSinceLastDirectionChange > randomDirectionChangeTime)
                {
                    h = Random.Range(-1f, 1f);
                    v = Random.Range(0f, 1f); // Sadece ileri hareket
                    timeSinceLastDirectionChange = 0f;
                }
                timeSinceLastDirectionChange += Time.fixedDeltaTime;
                break;
        }
        Move(v, h);

    }

    void Move(float acceleration, float steering)
    {
        float steeringAngle = maxSteerAngle * steering;
        float motorTorque = motorForce * acceleration;

        for (int i = 0; i < wheelColliders.Length; i++)
        {
            WheelCollider wheelCollider = wheelColliders[i];
            if (i < 2) // İlk iki tekerlek direksiyonlu varsayalım
            {
                wheelCollider.steerAngle = steeringAngle;
            }

            wheelCollider.motorTorque = motorTorque;
        }
    }

    public void SetPlayerControlling(bool playerControlling)
    {
        controlMode = ControlMode.Player;
        if (controlMode == ControlMode.Player)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            // rb.constraints = RigidbodyConstraints.None;
            // rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            roverCamera.gameObject.SetActive(true);
            player.gameObject.SetActive(false);
        }
        else
        {
            // rb.isKinematic = true;
            // rb.useGravity = false;
            // rb.constraints = RigidbodyConstraints.FreezeAll;
            roverCamera.gameObject.SetActive(false);
            player.gameObject.SetActive(true);
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

    private void ArmControl()
    {
        if (Input.GetKey(KeyCode.K))
        {
            armVertical.Rotate(0, 1, 0, Space.World);
        }
        if (Input.GetKey(KeyCode.H))
        {
            armVertical.Rotate(0, -1, 0, Space.World);
        }
        if (Input.GetKey(KeyCode.U))
        {
            armHorizontal.Rotate(0, 1, 0, Space.Self);
        }
        if (Input.GetKey(KeyCode.J))
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

    private void DropItem()
    {
        Collectable collectable = armCollider.GetComponentInChildren<Collectable>();
        if (collectable != null)
        {
            collectable.Drop();
        }
    }
}
