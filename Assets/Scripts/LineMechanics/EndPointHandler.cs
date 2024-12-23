using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPointHandler : MonoBehaviour
{
    public Transform endPoint;

    public bool IsLineTouchingEndPoint(List<Vector3> points)
    {
        foreach (Vector3 point in points)
        {
            float distance = Vector3.Distance(point, endPoint.position);
            if (distance <= 0.1f) // Pas deze straal aan indien nodig
            {
                return true;
            }
        }
        return false;
    }
}
