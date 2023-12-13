using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI stats;
    [SerializeField] CinemachineVirtualCamera thirdPersonCam;
    [SerializeField] CinemachineVirtualCamera lookBackCam;

    public float throttleIncrement = 0.1f;
    public float maxThrust = 200f;
    public float sensitivity = 200f;
    public float lift = 135f;

    public float minVolume = 0f;
    public float maxVolume = 0.2f;

    private float throttle;
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
    Propeller propeller;

    private void OnEnable()
    {
        CameraManager.Register(thirdPersonCam);
        CameraManager.Register(lookBackCam);
        CameraManager.SwitchCamera(thirdPersonCam);
    }

    private void OnDisable()
    {
        CameraManager.Unregister(thirdPersonCam);
        CameraManager.Unregister(lookBackCam);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        engineSound = GetComponent<AudioSource>();
        propeller = FindObjectOfType<Propeller>();
    }

    private void HandleInputs()
    {
        roll = Input.GetAxis("Horizontal");
        pitch = Input.GetAxis("Vertical");
        yaw = Input.GetAxis("Yaw");

        if (Input.GetKey(KeyCode.Space))
        {
            IncreaseThrottle();
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            DecreaseThrottle();
        }

        engineSound.volume = Mathf.Clamp(throttle * 0.01f, minVolume, maxVolume);

        throttle = Mathf.Clamp(throttle, 0f, 100f);

        propeller.speed = throttle * 50f;
    }

    private void Update()
    {
        HandleInputs();
        UpdateHUD();

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(1);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (CameraManager.IsActiveCamera(thirdPersonCam))
            {
                CameraManager.SwitchCamera(lookBackCam);
            }
            else if (CameraManager.IsActiveCamera(lookBackCam))
            {
                CameraManager.SwitchCamera(thirdPersonCam);
            }
        }
    }

    private void FixedUpdate()
    {
        // Apply forward force to the plane
        rb.AddForce(transform.forward * maxThrust * throttle);
        // Apply rotational force on the x-axis to turn the plane up and down
        rb.AddTorque(transform.right * pitch * sensitivityModifier);
        // Apply rotational force on the z-axis to roll the plane left and right
        rb.AddTorque(-transform.forward * roll * sensitivityModifier);
        // Apply rotational force on the y-axis to turn the plane left and right
        rb.AddTorque(transform.up * yaw * sensitivityModifier);

        // Apply upward force to lift the plane upwards
        rb.AddForce(Vector3.up * rb.velocity.magnitude * lift);
    }

    void IncreaseThrottle()
    {
        throttle += throttleIncrement;
    }

    void DecreaseThrottle()
    {
        throttle -= throttleIncrement;
    }

    private void UpdateHUD()
    {
        stats.text = "Throttle: " + throttle.ToString("F0") + "%\n"
            + "Airspeed: " + (rb.velocity.magnitude * 3.6f).ToString("F0") + "km/h\n"
            + "Altitude: " + transform.position.y.ToString("F0") + "m";
    }
}
