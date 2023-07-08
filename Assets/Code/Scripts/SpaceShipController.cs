using UnityEngine;
using UnityEngine.InputSystem;

public class SpaceShipController : MonoBehaviour
{
    public float forwardThrust = 10f;
    public float sideThrust = 5f;
    public float pitchTorque = 50f;
    public float rollTorque = 30f;

    private Rigidbody rb;
    private CockpitController controls;
    private Vector2 moveInput;
    private Vector2 rotateInput;
    private bool controlsEnabled = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        controls = new CockpitController();

        controls.Player.move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.move.canceled += ctx => moveInput = Vector2.zero;

        controls.Player.rotate.performed += ctx => rotateInput = ctx.ReadValue<Vector2>();
        controls.Player.rotate.canceled += ctx => rotateInput = Vector2.zero;
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
    }

    void MoveShip()
    {
        Vector3 forwardThrustVec = transform.forward * moveInput.y * forwardThrust;
        Vector3 sideThrustVec = transform.right * moveInput.x * sideThrust;

        Vector3 pitchTorqueVec = transform.right * -rotateInput.y * pitchTorque;
        Vector3 rollTorqueVec = transform.forward * -rotateInput.x * rollTorque;

        rb.AddForce(forwardThrustVec, ForceMode.Force);
        rb.AddForce(sideThrustVec, ForceMode.Force);

        rb.AddTorque(pitchTorqueVec, ForceMode.Force);
        rb.AddTorque(rollTorqueVec, ForceMode.Force);
    }

    public void ShakeShip()
    {
        rb.AddForce(Random.insideUnitSphere * 1000f, ForceMode.Impulse);
        rb.AddTorque(Random.insideUnitSphere * 3f, ForceMode.Impulse);
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
