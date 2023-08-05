using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class SpaceShipController : MonoBehaviour
{
    public float forwardThrust = 10f;
    public float sideThrust = 5f;
    public float pitchTorque = 50f;
    public float rollTorque = 30f;
    public float thrustForce = 20f;
    public float rollZTorque = 2000f;
    public float lockRotateSpeed = 0f;
    public float lockSpeed = 700f;

    public GameObject planet;
    public bool lockPlanet = false;
    public float distanceToLockedPlanet;


    private Rigidbody rb;
    private CockpitController controls;
    private Vector2 moveInput;
    private Vector2 rotateInput;
    private Vector2 rotateInputMouse;
    private float thrustInput;
    private float rollZInput;

    public GameObject particleSystem;

    private bool controlsEnabled = true;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        controls = new CockpitController();

        controls.Player.move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.move.canceled += ctx => moveInput = Vector2.zero;

        // //Old Rotation
        // controls.Player.rotate.performed += ctx => rotateInput = ctx.ReadValue<Vector2>();
        // controls.Player.rotate.canceled += ctx => rotateInput = Vector2.zero;

        //Mouse Rotation
        controls.Player.rotatemouse.performed += ctx => rotateInputMouse = ctx.ReadValue<Vector2>();
        controls.Player.rotatemouse.canceled += ctx => rotateInputMouse = Vector2.zero;

        controls.Player.thrust.performed += ctx => thrustInput = ctx.ReadValue<float>();
        controls.Player.thrust.canceled += ctx => thrustInput = 0f;

        controls.Player.roll.performed += ctx => rollZInput = ctx.ReadValue<float>();
        controls.Player.roll.canceled += ctx => rollZInput = 0f;

        controls.Player.locktoplanet.performed += _ =>
        {
            if (!lockPlanet)
            {
                rb.angularVelocity = Vector3.zero;
                controls.Player.rotatemouse.Disable();
                lockSpeed = 700f;
                StartCoroutine(StartParticleEffect());
            }
            else
            {
                controls.Player.rotatemouse.Enable();
                particleSystem.SetActive(false);
            }
            lockPlanet = !lockPlanet;
        };
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void FixedUpdate()
    {
        if (controlsEnabled)
        {
            MoveShip();
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            Teleport();
        }
        LockToPlanet();
    }

    void MoveShip()
    {


        Vector3 forwardThrustVec = transform.forward * moveInput.y * forwardThrust;
        Vector3 sideThrustVec = transform.right * moveInput.x * sideThrust;

        //Old Rotation
        //Vector3 pitchTorqueVec = transform.right * -rotateInput.y * pitchTorque;
        //Vector3 rollTorqueVec = transform.forward * -rotateInput.x * rollTorque;

        //Mouse Rotation
        Vector3 pitchTorqueVec = transform.right * -rotateInputMouse.y * pitchTorque;
        Vector3 rollTorqueVec = transform.up * rotateInputMouse.x * rollTorque;

        Vector3 thrustVector = transform.up * thrustInput * thrustForce;


        Vector3 rollZTorqueVec = transform.forward * rollZInput * rollZTorque;

        rb.AddForce(forwardThrustVec, ForceMode.Force);
        rb.AddForce(sideThrustVec, ForceMode.Force);

        rb.AddTorque(pitchTorqueVec, ForceMode.Force);
        rb.AddTorque(rollTorqueVec, ForceMode.Force);

        rb.AddForce(thrustVector, ForceMode.Force);

        rb.AddTorque(rollZTorqueVec, ForceMode.Force);

    }

    void Teleport()
    {
        transform.position = planet.transform.position + planet.transform.forward * 100f;
    }

    void LockToPlanet()
    {
        if (lockPlanet)
        {

            distanceToLockedPlanet = Vector3.Distance(planet.transform.position, transform.position);
            Vector3 targetDir = planet.transform.position - transform.position;
            float currDist = Vector3.Distance(planet.transform.position, transform.position);
            if (currDist <= 50f)
            {
                rb.velocity = new Vector3(0, 0, 50f);
                controls.Player.rotatemouse.Enable();
                lockPlanet = false;
                particleSystem.SetActive(false);
                return;
            }
            else if (currDist <= 200f)
            {
                lockSpeed = 500f;
            }
            rb.velocity = Vector3.zero;
            Quaternion targetRotation = Quaternion.LookRotation(targetDir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, lockRotateSpeed * Time.deltaTime);
            rb.AddForce(transform.forward * lockSpeed, ForceMode.VelocityChange);
        }

    }
    private IEnumerator StartParticleEffect()
    {
        yield return new WaitForSeconds(1.5f);
        if (!particleSystem.activeInHierarchy) particleSystem.SetActive(true);

    }

    // void LockToPlanet(GameObject planet)
    // {
    //     if (!lockPlanet)
    //     {
    //         lockPlanet = true;
    //         StartCoroutine(RotateTowardsPlanet(planet.transform.position, 3.0f));
    //     }
    // }

    //private IEnumerator RotateTowardsPlanet(Vector3 targetPosition, float duration)
    //{
    //    Vector3 directionToPlanet = (targetPosition - transform.position).normalized;
    //    Quaternion targetRotation = Quaternion.LookRotation(directionToPlanet);

    //    float elapsedTime = 0.0f;

    //    while (elapsedTime < duration)
    //    {
    //        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, elapsedTime / duration);
    //        elapsedTime += Time.deltaTime;
    //        yield return null;
    //    }

    //    transform.rotation = targetRotation;
    //    lockPlanet = false;
    //}


    public void ShakeShip()
    {
        rb.AddForce(Random.insideUnitSphere * 300f, ForceMode.Impulse);
        rb.AddTorque(Random.insideUnitSphere * 3000f, ForceMode.Impulse);
    }

    public void DisableControls()
    {
        controlsEnabled = false;
    }

    public void EnableControls()
    {
        controlsEnabled = true;
    }
}
