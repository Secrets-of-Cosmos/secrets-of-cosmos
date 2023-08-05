using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingenuity : MonoBehaviour
{
    public Transform rotors_01;
    public Transform rotors_02;
    public float speed = 5f;
    public float maxRotationSpeed = 1000f;
    private float rotationSpeed = 0f;
    public float maxHeight = 50f;
    private bool flying = false;
    private bool landing = false;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        MoveUp();
        RotateRotor();
    }

    private void MoveUp()
    {
        if (transform.position.y > maxHeight)
        {
            Land();
        }
        if (flying && transform.position.y < maxHeight)
        {
            rb.AddForce(Vector3.up * rotationSpeed * Time.deltaTime, ForceMode.Acceleration);

            if (rotationSpeed < maxRotationSpeed)
            {
                rotationSpeed = Mathf.Min(maxRotationSpeed, rotationSpeed + 1f);
            }
        }
        if (landing)
        {
            rb.AddForce(Vector3.up * rotationSpeed * Time.deltaTime, ForceMode.Acceleration);

            if (rotationSpeed > 0)
            {
                rotationSpeed = rotationSpeed - 1f;
            }
            else
            {
                landing = false;
            }
        }
    }

    private void RotateRotor()
    {
        rotors_01.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);
        rotors_02.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);
    }

    public void Fly()
    {
        flying = true;
    }

    public void Land()
    {
        flying = false;
        landing = true;
    }

}
