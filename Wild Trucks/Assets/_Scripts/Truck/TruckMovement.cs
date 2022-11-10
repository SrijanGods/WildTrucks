using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckMovement : MonoBehaviour
{
    public GameObject endPoint;

    public bool look;

    public bool collided;

    public int speed; 
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        endPoint = GameObject.FindGameObjectWithTag("EndPoint");
        if(look)
            gameObject.transform.LookAt(endPoint.transform);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Quaternion targetRotation = Quaternion.LookRotation(endPoint.transform.position - transform.position);
        if(!collided)
            transform.position += transform.forward * speed * 1f * Time.deltaTime;
        else
            transform.GetComponent<Rigidbody>().AddRelativeForce(transform.forward * 100 * speed);
    }

    private void Update()
    {

    }
}
