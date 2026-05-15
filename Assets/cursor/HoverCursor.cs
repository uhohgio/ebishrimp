using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverCursor : MonoBehaviour , IPointerEnterHandler , IPointerExitHandler
{
    // public Texture2D customCursor;
    public Texture2D customCursorHover;
    
    void Start()
    {
        // Cursor.SetCursor(customCursor, Vector2.zero, CursorMode.Auto);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(customCursorHover, Vector2.zero, CursorMode.Auto);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Cursor.SetCursor(customCursor, Vector2.zero, CursorMode.Auto);
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}