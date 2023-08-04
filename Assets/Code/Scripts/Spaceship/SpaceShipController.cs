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

    public GameObject planet;
    private bool lockedToPlanet = false;

    private Rigidbody rb;
    private CockpitController controls;
    private Vector2 moveInput;
    private Vector2 rotateInput;
    private Vector2 rotateInputMouse;
    private float thrustInput;
    private float rollZInput;

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
        if(Input.GetKeyDown(KeyCode.V))
        {
            Teleport();
        }
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

    void Teleport() {
        transform.position = planet.transform.position + planet.transform.forward * 100f;
    }

    // void LockToPlanet(GameObject planet)
    // {
    //     if (!lockedToPlanet)
    //     {
    //         lockedToPlanet = true;
    //         StartCoroutine(RotateTowardsPlanet(planet.transform.position, 3.0f));
    //     }
    // }

    // private IEnumerator RotateTowardsPlanet(Vector3 targetPosition, float duration)
    // {
    //     Vector3 directionToPlanet = (targetPosition - transform.position).normalized;
    //     Quaternion targetRotation = Quaternion.LookRotation(directionToPlanet);

    //     float elapsedTime = 0.0f;

    //     while (elapsedTime < duration)
    //     {
    //         transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, elapsedTime / duration);
    //         elapsedTime += Time.deltaTime;
    //         yield return null;
    //     }

    //     transform.rotation = targetRotation;
    //     lockedToPlanet = false;
    // }


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
