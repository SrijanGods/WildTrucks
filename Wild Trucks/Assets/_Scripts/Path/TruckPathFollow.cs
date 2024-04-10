using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckPathFollow : MonoBehaviour
{
    public float maxSteerAngle = 45f;
    public float speedTorque;
    [Space]
    public WheelCollider FL;
    public WheelCollider FR;
    public GameObject COM;
    public int maxSpeed;
    [Space, HideInInspector]
    public GameObject path;

    public List<Transform> nodes = new List<Transform>();

    private int currentNode = 0;
    private int nodeLength;

    LevelManager levelManager;
    

    Rigidbody rb;

    [HideInInspector]
    public Vector3 movementSpeed;

    private void Start()
    {
        //GetComponent<Rigidbody>().centerOfMass = COM.transform.position;
        path = GameObject.FindGameObjectWithTag("Path").gameObject;

        Transform[] pathPoints = path.GetComponentsInChildren<Transform>();
        nodes.Clear();

        nodeLength = pathPoints.Length;
        for (int i = 0; i < pathPoints.Length; i++)
        {
            if (pathPoints[i] != path.transform)
            {
                nodes.Add(pathPoints[i]);
            }
        }

        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        maxSpeed = levelManager.maxSpeed;

        rb = gameObject.GetComponent<Rigidbody>();
        //rb.centerOfMass = COM.transform.position;

    }

    private void FixedUpdate()
    {
        
        WheelAngle();

        movementSpeed = rb.velocity;

        if (rb.velocity.magnitude <= maxSpeed)
        {
            Drive();
        }
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            Vector3 limitedVel = flatVel.normalized * maxSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }

        CheckWayPointDist();
    }

    void WheelAngle()
    {
        /*
        Vector3 relVec = transform.InverseTransformPoint(nodes[currentNode].position);
        float newSteer = (relVec.x / relVec.magnitude) * maxSteerAngle;
        Debug.LogError(newSteer);

        FL.steerAngle = newSteer;
        FR.steerAngle = newSteer;
        */

        Vector3 nodeRelP = nodes[currentNode].position - transform.position;
        Vector3 ourPos = transform.forward;

        float newSteer = Vector3.SignedAngle(nodeRelP, ourPos, Vector3.up);
        //Debug.Log(newSteer);
        FL.steerAngle = -newSteer;
        FR.steerAngle = -newSteer;

    }

    void Drive()
    {
        FL.motorTorque = speedTorque * 500;
        FR.motorTorque = speedTorque * 500;
        
    }

    void CheckWayPointDist()
    {
        if(Vector3.Distance(transform.position, nodes[currentNode].position) < 10f)
        {
            if(currentNode != nodes.Count - 1)
            {
                currentNode++;
            }
        }
    }

    void TruckDeath()
    {
        gameObject.GetComponent<TruckPathFollow>().enabled = false;
        rb.mass = 1;
        rb.drag = 0;
    }
}
