using UnityEngine;
using System.Collections;

public class RoverManual : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 30f;
    public float groundRayLength = 1.5f;
    public Transform[] groundRayOrigins;
    public Transform[] wheelTransforms;
    public LayerMask groundLayer;
    public LayerMask obstacleLayer; // Engelleri tespit etmek için
    public float timeToChangeDirection = 2f;
    private float directionChangeInterval;
    private float surfaceAlignmentTime = 1f; // Yüzeyle uyum sağlama süresi (saniye)
    private Quaternion targetRotation;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        targetRotation = transform.rotation; // Başlangıç dönüşünü hedef olarak ayarla
        directionChangeInterval = timeToChangeDirection;
    }

    void Update()
    {
        Move();
        if (transform.rotation != targetRotation)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, surfaceAlignmentTime * Time.deltaTime);
        }
    }

    void Move()
    {
        if (!DetectObstacle())
        {
            AlignToSurface();
            SpeedControl();
            WheelRotation();
        }
        else
        {
            RotateRandomly();
        }
    }

    void RotateRandomly()
    {
        directionChangeInterval -= Time.deltaTime;
        if (directionChangeInterval < 0)
        {
            directionChangeInterval = timeToChangeDirection;
            float angle = Random.Range(-90f, 90f);
            targetRotation *= Quaternion.AngleAxis(angle, Vector3.up);
        }
    }

    void WheelRotation()
    {
        if (rb.velocity.magnitude > 0)
        {
            foreach (Transform wheel in wheelTransforms)
            {
                wheel.Rotate(-rb.velocity.magnitude * Time.deltaTime * 50f, 0f, 0f);
            }
        }
    }

    void SpeedControl()
    {
        if (rb.velocity.magnitude < speed)
        {
            Vector3 force = transform.forward * speed;
            rb.AddForce(force, ForceMode.VelocityChange);
        }
    }

    bool DetectObstacle()
    {
        // Rover'ın önündeki engelleri tespit et
        return Physics.Raycast(transform.position, transform.forward, groundRayLength, obstacleLayer);
    }

    void AlignToSurface()
    {
        Vector3 averageNormal = Vector3.zero;

        foreach (Transform groundRayOrigin in groundRayOrigins)
        {
            RaycastHit[] hits;
            hits = Physics.RaycastAll(groundRayOrigin.position, -groundRayOrigin.up, groundRayLength, groundLayer);
            foreach (RaycastHit hit in hits)
            {
                averageNormal += hit.normal;
            }
        }

        if (groundRayOrigins.Length > 0)
        {
            averageNormal /= groundRayOrigins.Length;
            Quaternion newRotation = Quaternion.FromToRotation(transform.up, averageNormal) * transform.rotation;
            targetRotation = newRotation;
        }
    }
}
