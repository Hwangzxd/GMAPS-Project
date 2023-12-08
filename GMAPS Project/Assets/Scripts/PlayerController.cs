using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float thrust;
    public float thrustMultiplier;
    public float yawMultiplier;
    public float pitchMultiplier;
    new Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float pitch = Input.GetAxis("Vertical");
        float yaw = Input.GetAxis("Horizontal");

        rb.AddRelativeForce(0f, 0f, thrust * thrustMultiplier * Time.deltaTime);
        rb.AddRelativeTorque(pitch * pitchMultiplier * Time.deltaTime,
        yaw * yawMultiplier * Time.deltaTime, -yaw * yawMultiplier * 2 * Time.deltaTime);
    }
}
