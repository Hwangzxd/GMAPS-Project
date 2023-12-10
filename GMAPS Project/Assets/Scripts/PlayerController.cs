using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    //public float thrust;
    public float thrustMultiplier = 0.1f;
    public float yawMultiplier = 0.1f;
    public float pitchMultiplier = 0.1f;
    public float maxThrust = 200f;
    public float sensitivity = 10f;
    public float lift = 135f;

    private float thrust;
    private float roll;
    private float pitch;
    private float yaw;

    private float sensitivityModifier
    {
        get
        {
            return (rb.mass / 10f) * sensitivity;
        }
    }

    Rigidbody rb;
    AudioSource engineSound;
    //Propeller propeller;
    [SerializeField] TextMeshProUGUI stats;
    [SerializeField] Transform propeller;

    public CameraManager cameraManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        engineSound = GetComponent<AudioSource>();
        //propeller = FindObjectOfType<Propeller>();
    }

    private void HandleInputs()
    {
        roll = Input.GetAxis("Horizontal");
        pitch = Input.GetAxis("Vertical");
        yaw = Input.GetAxis("Yaw");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            thrust += thrustMultiplier;
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            thrust -= thrustMultiplier;
        }
        thrust = Mathf.Clamp(thrust, 0f, 100f);
    }

    private void Update()
    {
        HandleInputs();
        UpdateHUD();

        propeller.Rotate(Vector3.right * thrust);
        //engineSound.volume = thrust * 0.01f;

        if (Input.GetKeyDown(KeyCode.C))
        {
            cameraManager.SwitchCamera(cameraManager.thirdPersonCamera);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            cameraManager.SwitchCamera(cameraManager.lookBackCamera);
        }
    }

    private void FixedUpdate()
    {
        //float pitch = Input.GetAxis("Vertical");
        //float yaw = Input.GetAxis("Horizontal");

        //rb.AddRelativeForce(0f, 0f, thrust * thrustMultiplier * Time.deltaTime);
        //propeller.speed = thrustMultiplier * 1500f;
        //rb.AddRelativeTorque(pitch * pitchMultiplier * Time.deltaTime,
        //yaw * yawMultiplier * Time.deltaTime, -yaw * yawMultiplier * 2 * Time.deltaTime);

        rb.AddForce(transform.forward * maxThrust * thrust);
        engineSound.volume = thrust * 0.01f;
        rb.AddTorque(transform.right * pitch * sensitivityModifier);
        rb.AddTorque(-transform.forward * roll * sensitivityModifier);
        rb.AddTorque(transform.up * yaw * sensitivityModifier);

        rb.AddForce(Vector3.up * rb.velocity.magnitude * lift);
    }

    private void UpdateHUD()
    {
        stats.text = "Thrust: " + thrust.ToString("F0") + "%\n"
            + "Airspeed: " + (rb.velocity.magnitude * 3.6f).ToString("F0") + "km/h\n"
            + "Altitude: " + transform.position.y.ToString("F0") + "m";
    }
}
