using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Rover : MonoBehaviour
{
    public float motorTorque = 200f;
    public float maxSteerAngle = 30f;
    public WheelCollider[] frontWheelColliders;
    public WheelCollider[] rearWheelColliders;
    public Transform[] wheelMeshes;

    public float randomRotationInterval = 5f;
    private float randomRotationTimer = 0f;
    private float steerAngle = 0f;

    private void FixedUpdate()
    {
        Move();

        randomRotationTimer += Time.deltaTime;

        if (randomRotationTimer >= randomRotationInterval)
        {
            randomRotationTimer = 0f;
            steerAngle = Random.Range(-maxSteerAngle, maxSteerAngle);
        }
    }

    private void Move()
    {
        float motorForce = motorTorque;

        for (int i = 0; i < frontWheelColliders.Length; i++)
        {
            frontWheelColliders[i].steerAngle = steerAngle;
        }

        for (int i = 0; i < rearWheelColliders.Length; i++)
        {
            rearWheelColliders[i].motorTorque = motorForce;
            wheelMeshes[i + 2].Rotate(rearWheelColliders[i].rpm / 60 * 360 * Time.deltaTime, 0, 0);
        }
    }


}
