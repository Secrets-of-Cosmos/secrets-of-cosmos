using UnityEngine;
using System.Collections;

public class Rover : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 30f;
    public float groundRayLength = 1.5f;
    public Transform groundRayOrigin;
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
    }

    void Update()
    {

        Move();
        if (transform.rotation != targetRotation)
        {
            // LeanTween.rotate(gameObject, targetRotation.eulerAngles, surfaceAlignmentTime);
            // Yüzey normali ile uyumlu hale getiriliyor
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, surfaceAlignmentTime * Time.deltaTime);
        }
    }

    void Move()
    {
        // RotateRandomly();
        AlignToSurface();
        SpeedControl();
        WheelRotation();
    }

    void RotateRandomly()
    {
        directionChangeInterval -= Time.deltaTime;
        if (directionChangeInterval < 0)
        {
            directionChangeInterval = timeToChangeDirection;
            // Rastgele bir yön seç
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

    void AlignToSurface()
    {
        RaycastHit hit;
        if (Physics.Raycast(groundRayOrigin.position, -groundRayOrigin.up, out hit, groundRayLength, groundLayer))
        {
            Debug.Log("Yüzey normali ile uyumlu hale getiriliyor");
            // Yüzey normali ile uyumlu hale getiriliyor
            Quaternion newRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            targetRotation = newRotation;
        }
    }
}
