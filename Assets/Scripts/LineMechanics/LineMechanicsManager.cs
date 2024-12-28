using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineMechanicsManager : MonoBehaviour
{
    public RectTransform startPoint;
    public RectTransform endPoint;

    private StartPointHandler startPointHandler;
    private EndPointHandler endPointHandler;
    private LineDrawer lineDrawer;

    private bool isDrawing;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        startPointHandler = gameObject.AddComponent<StartPointHandler>();
        endPointHandler = gameObject.AddComponent<EndPointHandler>();
        lineDrawer = GetComponent<LineDrawer>();

        startPointHandler.startPoint = startPoint;
        endPointHandler.endPoint = endPoint;
    }

    void Update()
    {
        if (Time.timeScale == 0f) // Stopte tekenen als tijd gepauzeerd is
            return;

        if (Input.GetMouseButtonDown(0))
        {
            if (startPointHandler.IsMouseOverStartPoint())
            {
                isDrawing = true;
                lineDrawer.StartDrawing(startPoint.position);
            }
        }

        if (Input.GetMouseButton(0) && isDrawing)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 2f; // Zorg dat de z-waarde juist is voor je 2D lijn
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos);
            lineDrawer.UpdateLine(worldPos);
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDrawing = false;
        }

        if (isDrawing && endPointHandler.IsLineTouchingEndPoint(lineDrawer.GetPoints()))
        {
            // Gebruik LoadNextScene.Instance om naar de volgende scène te gaan
            LoadNextScene.Instance.LoadNextSceneProcess();

            isDrawing = false;
        }
    }
}
