using UnityEngine;
using System.Collections;

public class TestTruck : MonoBehaviour
{
    public float torque;
    [Space]
    public WheelCollider FL;
    public WheelCollider FR;
    [Space]
    public Transform node;
    public GameObject COM;

    private Rigidbody rb;
    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.centerOfMass = COM.transform.position;
    }
    private void FixedUpdate()
    {
        Vector3 nodeRelP = node.position - transform.position;
        Vector3 ourPos = transform.forward;

        float newSteer = Vector3.SignedAngle(nodeRelP, ourPos, Vector3.up);
        //Debug.Log(newSteer);
        FL.steerAngle = -newSteer;
        FR.steerAngle = -newSteer;

        FL.motorTorque = torque;
        FR.motorTorque = torque;
    }
}
