using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPointHandler : MonoBehaviour
{
    public RectTransform endPoint; // Gebruik nu een RectTransform
    private Canvas canvas;

    void Start()
    {
        canvas = endPoint.GetComponentInParent<Canvas>(); // Haal de Canvas op (nodig voor conversie)
    }

    public bool IsLineTouchingEndPoint(List<Vector3> points)
    {
        foreach (Vector3 point in points)
        {
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(point); // Converteer punt naar schermruimte

            // Controleer of het punt binnen de RectTransform valt
            if (RectTransformUtility.RectangleContainsScreenPoint(endPoint, screenPoint, canvas.worldCamera))
            {
                return true;
            }
        }
        return false;
    }
}

