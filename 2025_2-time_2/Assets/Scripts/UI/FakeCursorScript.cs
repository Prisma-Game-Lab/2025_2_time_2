using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class FakeCursorScript : MonoBehaviour
{
    [SerializeField] private CanvasScaler canvasScaler;
    RectTransform rectTransform;

    // Start is called before the first frame update
    private void Start()
    {
        Cursor.visible = false;
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnDestroy()
    {
        Cursor.visible = true;
    }

    public void MousePosUpdate(InputAction.CallbackContext context) 
    {
        if (Camera.main != null)
        { 
            Vector2 mousePos = context.ReadValue<Vector2>();
            mousePos = Camera.main.ScreenToViewportPoint(mousePos);
            mousePos *= canvasScaler.referenceResolution;
            rectTransform.anchoredPosition = mousePos;
        }
    }
}
