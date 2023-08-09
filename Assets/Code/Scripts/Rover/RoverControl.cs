using UnityEngine;

public class RoverControl : MonoBehaviour
{
    public float motorForce = 200f;
    public float maxSteerAngle = 30f;
    public WheelCollider[] wheelColliders;

    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Move(v, h);
    }

    void Move(float acceleration, float steering)
    {
        float steeringAngle = maxSteerAngle * steering;
        float motorTorque = motorForce * acceleration;

        // Ön tekerleklerin direksiyon açısını ve arka tekerleklerin motor torkunu ayarla
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
}
