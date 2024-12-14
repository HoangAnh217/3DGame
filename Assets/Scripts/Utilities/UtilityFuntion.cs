using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilityFuntion 
{
    public static Vector3 ConvertWordSpaceToUI(Vector3 worldPosition,Canvas canvas)
    {
        // Ki?m tra các ði?u ki?n c?n thi?t
        if (Camera.main == null)
        {
            Debug.LogError("Main Camera is not assigned!");
            return Vector3.zero;
        }

        if (canvas == null)
        {
            Debug.LogError(" Canvas is null!");
            return Vector3.zero;
        }

        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

        // L?y RectTransform c?a Canvas
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();

        // Chuy?n ð?i t? Screen Space sang Local Space c?a Canvas
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPosition,
            canvas.worldCamera,
            out Vector2 canvasPosition
        );

        // Tr? v? v? trí trong UI Space (Canvas Space)
        return canvasPosition;
    }
}
