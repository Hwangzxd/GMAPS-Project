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

    private float throttle;
    private float roll;
    private float pitch;
    private float yaw;

    //bool brake = false;

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
            throttle += throttleIncrement;
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            throttle -= throttleIncrement;
        }
        throttle = Mathf.Clamp(throttle, 0f, 100f);

        propeller.speed = throttle * 50f;

        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    brake = !brake;
        //}
    }

    private void Update()
    {
        HandleInputs();
        UpdateHUD();

        if (engineSound.volume < 0.2f)
        {
            engineSound.volume = throttle * 0.01f;
        }
        
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
        //float pitch = Input.GetAxis("Vertical");
        //float yaw = Input.GetAxis("Horizontal");

        //rb.AddRelativeForce(0f, 0f, thrust * thrustMultiplier * Time.deltaTime);
        //propeller.speed = thrustMultiplier * 1500f;
        //rb.AddRelativeTorque(pitch * pitchMultiplier * Time.deltaTime,
        //yaw * yawMultiplier * Time.deltaTime, -yaw * yawMultiplier * 2 * Time.deltaTime);

        rb.AddForce(transform.forward * maxThrust * throttle);
        rb.AddTorque(transform.right * pitch * sensitivityModifier);
        rb.AddTorque(-transform.forward * roll * sensitivityModifier);
        rb.AddTorque(transform.up * yaw * sensitivityModifier);

        rb.AddForce(Vector3.up * rb.velocity.magnitude * lift);
    }

    //public void Brake(bool isBraking) //increases drag on wheels
    //{
    //    //add drag on wheels
    //    SphereCollider[] wheels = FindObjectsOfType<SphereCollider>();

    //    //change based on isBraking
    //    float friction;
    //    if (isBraking)
    //    {
    //        friction = 0.2f;
    //    }
    //    else
    //    {
    //        friction = 0f;
    //    }

    //    foreach (SphereCollider wheel in wheels)
    //    {
    //        wheel.material.dynamicFriction = friction;
    //    }
    //}

    private void UpdateHUD()
    {
        stats.text = "Throttle: " + throttle.ToString("F0") + "%\n"
            + "Airspeed: " + (rb.velocity.magnitude * 3.6f).ToString("F0") + "km/h\n"
            + "Altitude: " + transform.position.y.ToString("F0") + "m";
    }
}
