using UnityEngine;

public class RoverControl : MonoBehaviour
{
    public enum ControlMode { Player, Random }
    public ControlMode controlMode = ControlMode.Player;

    public float motorForce = 200f;
    public float maxSteerAngle = 30f;
    public float randomDirectionChangeTime = 3f; // Rastgele yön değişikliği süresi

    public WheelCollider[] wheelColliders;

    private float h, v;
    private float timeSinceLastDirectionChange;

    void FixedUpdate()
    {
        switch (controlMode)
        {
            case ControlMode.Player:
                h = Input.GetAxis("Horizontal");
                v = Input.GetAxis("Vertical");
                break;

            case ControlMode.Random:
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
}
