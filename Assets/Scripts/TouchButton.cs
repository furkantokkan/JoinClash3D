using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private static bool pressing;

    public static bool IsPressing()
    {
        return pressing;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pressing = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        pressing = false;
    }
    public static float GetMouseX()
    {
        return Input.mousePosition.x / (float)Screen.width;
    }
}
