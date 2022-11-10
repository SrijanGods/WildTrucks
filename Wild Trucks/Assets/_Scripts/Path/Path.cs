using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Path : MonoBehaviour
{
    public Color pathColour;

    public List<Transform> nodes = new List<Transform>();

    private void OnDrawGizmos()
    {
        Gizmos.color = pathColour;

        Transform[] pathPoints = GetComponentsInChildren<Transform>();
        nodes.Clear();

        for(int i = 0; i < pathPoints.Length; i++)
        {
            if(pathPoints[i] != transform)
            {
                nodes.Add(pathPoints[i]);
            }
        }

        for(int i = 0; i < nodes.Count; i++)
        {
            Vector3 thisNode = nodes[i].position;
            Vector3 lastNode = Vector3.zero;

            if (i > 0)
            {
                lastNode = nodes[i - 1].position;
                Gizmos.DrawLine(thisNode, lastNode);
            }

            
        }
    }
}
