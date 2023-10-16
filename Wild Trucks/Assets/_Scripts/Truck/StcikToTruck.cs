using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StcikToTruck : MonoBehaviour
{
    public bool OnPlatform;
    public GameObject otherGameObject;

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.tag == "Truck")
        {
            otherGameObject = other.transform.parent.gameObject;
            OnPlatform = true;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.transform.parent.tag == "Truck")
        {
            OnPlatform = false;
        }
    }
    public void FixedUpdate()
    {
        if (OnPlatform)
        {
            //transform.position += otherGameObject.transform.forward * Time.deltaTime * otherGameObject.GetComponent<TruckPathFollow>().movementSpeed;
        }
    }
}
