using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPointHandler : MonoBehaviour
{
    public RectTransform startPoint; // Gebruik nu een RectTransform
    private Canvas canvas;

    void Start()
    {
        canvas = startPoint.GetComponentInParent<Canvas>(); // Haal de Canvas op (nodig voor conversie)
    }

    public bool IsMouseOverStartPoint()
    {
        Vector2 mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Input.mousePosition,
            canvas.worldCamera,
            out mousePos
        );

        // Controleer of de muis binnen de RectTransform valt
        return RectTransformUtility.RectangleContainsScreenPoint(startPoint, Input.mousePosition, canvas.worldCamera);
    }
}
