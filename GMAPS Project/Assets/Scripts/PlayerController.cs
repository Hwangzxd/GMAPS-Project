using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float thrust;
    public float thrustMultiplier;
    public float yawMultiplier;
    public float pitchMultiplier;

    Rigidbody rb;
    Propeller propeller;

    public CameraManager cameraManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        propeller = FindObjectOfType<Propeller>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            cameraManager.SwitchCamera(cameraManager.thirdPersonCamera);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            cameraManager.SwitchCamera(cameraManager.sideViewCamera);
        }
    }

    private void FixedUpdate()
    {
        float pitch = Input.GetAxis("Vertical");
        float yaw = Input.GetAxis("Horizontal");

        rb.AddRelativeForce(0f, 0f, thrust * thrustMultiplier * Time.deltaTime);
        propeller.speed = thrustMultiplier * 1500f;
        rb.AddRelativeTorque(pitch * pitchMultiplier * Time.deltaTime,
        yaw * yawMultiplier * Time.deltaTime, -yaw * yawMultiplier * 2 * Time.deltaTime);
    }
}
