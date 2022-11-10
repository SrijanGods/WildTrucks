using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    private Rigidbody rigidbody;

    public float neededVel;
    public GameObject truck;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = transform.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(truck != null)
        {
            Rigidbody truckR = truck.transform.GetComponent<Rigidbody>();
            /*
            float lastVel = 0;
            
            float truckAcc = (truckR.velocity.magnitude - lastVel) / Time.deltaTime;
            lastVel = truckR.velocity.magnitude;

            float force = truckR.mass * truckAcc;
            rigidbody.AddForce();
            
            
            var pointVel = truckR.GetPointVelocity(truck.transform.position);
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, pointVel.z);
            print(rigidbody.velocity.z + " " + truckR.velocity.z);
            */


        }
    }

    public void SetRigidbody(GameObject saidTruck)
    {
        //rigidbody.velocity += rigidbody.velocity - vel;
        //neededVel = vel;
        truck = saidTruck;
        //rigidbody.AddForce(new Vector3(0,0,saidTruck.GetComponent<Rigidbody>().mass));
    }
}
