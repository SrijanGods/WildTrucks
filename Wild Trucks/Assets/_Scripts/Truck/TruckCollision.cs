using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckCollision : MonoBehaviour
{
    public bool sideCollider;
    //public GameObject playerHolder;

    private Transform otherP;
    private void Start()
    {
        //playerHolder
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.transform.SetParent(this.transform.parent.transform);
            if (sideCollider)
            {
                other.transform.GetComponent<Jump>().isSideColl = true;
            }
            other.transform.GetComponent<PlayerPhysics>().SetRigidbody(transform.parent.gameObject);
        }
        if (other.tag == "Collider")
        {
            transform.GetComponentInParent<TruckMovement>().collided = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.GetComponent<Jump>().isAttached = true;
        }
        if (other.tag == "Collider")
        {
            transform.GetComponentInParent<TruckMovement>().collided = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.GetComponent<Jump>().isAttached = false;
            //other.transform.SetParent(null);

            otherP = other.transform;
            if (sideCollider)
            {
                other.transform.GetComponent<Jump>().isSideColl = false;
            }

            other.transform.GetComponent<PlayerPhysics>().SetRigidbody(transform.parent.gameObject);
            
        }
        if (other.tag == "Collider")
        {
            transform.GetComponentInParent<TruckMovement>().collided = true;
        }
    }
}
