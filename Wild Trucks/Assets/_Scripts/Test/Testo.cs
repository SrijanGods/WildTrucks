using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testo : MonoBehaviour
{
    public int force;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 1f * Time.deltaTime * transform.GetComponent<Rigidbody>().mass * force);
    }
}
