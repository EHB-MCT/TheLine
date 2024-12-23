using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPointHandler : MonoBehaviour
{
    public Transform startPoint;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    public bool IsMouseOverStartPoint()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f; // Afstand van de camera
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos);

        float distance = Vector3.Distance(worldPos, startPoint.position);
        return distance <= 0.5f; // Pas deze straal aan indien nodig
    }
}

