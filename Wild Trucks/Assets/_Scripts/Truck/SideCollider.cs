using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideCollider : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.name == "PlayerT")
        {
            other.gameObject.GetComponent<PlayerMovementTutorial>().isSide = true;
        }
    }
}
