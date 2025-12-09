using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WindowController : MonoBehaviour
{
    [SerializeField] private RectTransform canvas;
    [SerializeField] private RectTransform window;
    [SerializeField] private GameObject holder;
    [SerializeField] private RectTransform topBar;

    [SerializeField] private float minWidth;
    [SerializeField] private float minHeight;

    private bool movingWindow;
    private bool resizingWindow;
    private bool resizingWindowX;
    private bool resizingWindowY;
    private Vector2 clickOffset;
    private Vector2 mouseDiff;

    private void Start()
    {
        if (window == null)
            window = GetComponent<RectTransform>();
    }

    private void FixedUpdate()
    {
        if (movingWindow)
            MoveWindow();
        else if (resizingWindow)
            ResizeWindow();
    }

    public void ToggleWindow() 
    {
        if (!holder.activeInHierarchy)
            OpenWindow();
        else
            CloseWindow();
    }

    public void OpenWindow() 
    {
        holder.SetActive(true);
    }

    public void CloseWindow() 
    {
        holder.SetActive(false);
    } 

    public void EnableWindowMovement() 
    {
        movingWindow = true;
        Vector2 mousePos = Mouse.current.position.value;
        clickOffset = (Vector2)transform.position - mousePos;
    }

    public void DisableWindowMovement() 
    {
        movingWindow = false;
    }

    private void MoveWindow() 
    {
        transform.position = Mouse.current.position.value + clickOffset;
        ClampPosition();
    }

    private void ClampPosition() 
    {
        Vector2 clampedPos = transform.position;
        Vector2 relativePos = transform.position - topBar.position;

        if (topBar.transform.position.x < 0)
            clampedPos.x = relativePos.x;
        else if (topBar.transform.position.x > canvas.sizeDelta.x)
            clampedPos.x = canvas.sizeDelta.x + relativePos.x;

        float correctedY = topBar.transform.position.y - topBar.sizeDelta.y / 2;
        float yDif = correctedY - transform.position.y;
        if (correctedY < 0)
            clampedPos.y = -yDif;
        else if (correctedY > canvas.sizeDelta.y) 
            clampedPos.y = canvas.sizeDelta.y - yDif;

        transform.position = clampedPos;
    }

    public void StartRezise() 
    {
        resizingWindow = true;

        Vector2 mousePos = Mouse.current.position.value;
        mouseDiff = (mousePos - (Vector2)transform.position) / window.sizeDelta * 2;

        Vector2 absMouseDiff = new Vector2(Mathf.Abs(mouseDiff.x), Mathf.Abs(mouseDiff.y));

        if (absMouseDiff.x > absMouseDiff.y) 
        {
            if (absMouseDiff.y > 0.8f)
            {
                StartResizeY();
            }
            StartResizeX();
        }
        else 
        {
            if (absMouseDiff.x > 0.8f)
            {
                StartResizeX();
            }
            StartResizeY();
        }
    }

    private void StartResizeX() 
    {
        resizingWindowX = true;

        if (mouseDiff.x < 0)
        {
            transform.position += new Vector3(window.rect.width / 2, 0);
            window.pivot = new Vector2(1, window.pivot.y);
        }
        else
        {
            transform.position -= new Vector3(window.rect.width / 2, 0);
            window.pivot = new Vector2(0, window.pivot.y);
        }
    }

    private void StartResizeY()
    {
        resizingWindowY = true;

        if (mouseDiff.y < 0)
        {
            transform.position += new Vector3(0, window.rect.height / 2);
            window.pivot = new Vector2(window.pivot.x, 1);
        }
        else
        {
            transform.position -= new Vector3(0, window.rect.height / 2);
            window.pivot = new Vector2(window.pivot.x, 0);
        }
    }

    public void StopRezise() 
    {
        if (resizingWindowX)
        {
            StopResizeX();
        }
        if (resizingWindowY)
        {
            StopResizeY();
        }

        window.pivot = new Vector2(0.5f, 0.5f);
        
        resizingWindow = false;
    }

    private void StopResizeX()
    {
        resizingWindowX = false;

        if (mouseDiff.x < 0)
        {
            transform.position -= new Vector3(window.rect.width / 2, 0);
        }
        else
        {
            transform.position += new Vector3(window.rect.width / 2, 0);
        }
    }

    private void StopResizeY()
    {
        resizingWindowY = false;

        if (mouseDiff.y < 0)
        {
            transform.position -= new Vector3(0, window.rect.height / 2);
        }
        else
        {
            transform.position += new Vector3(0, window.rect.height / 2);
        }
    }

    private void ResizeWindow() 
    {
        Vector2 mousePos = Mouse.current.position.value;
        Vector2 relativeSize = (Vector2)transform.position - mousePos;

        ClampSize(ref relativeSize.x, mouseDiff.x, resizingWindowX);
        ClampSize(ref relativeSize.y, mouseDiff.y, resizingWindowY);

        if (resizingWindowX)
            relativeSize.x = Mathf.Max(Mathf.Abs(relativeSize.x), minWidth);
        else
            relativeSize.x = window.sizeDelta.x;

        if (resizingWindowY)
            relativeSize.y = Mathf.Max(Mathf.Abs(relativeSize.y), minHeight);
        else
            relativeSize.y = window.sizeDelta.y;
        
        window.sizeDelta = relativeSize;
    }

    private void ClampSize(ref float valueSize, float mouseDiffAxis, bool condition) 
    {
        if (condition)
        {
            if (mouseDiffAxis < 0)
            {
                if (valueSize < 0)
                    valueSize = 0;
            }
            else
            {
                if (valueSize > 0)
                    valueSize = 0;
            }
        }
        else
        {
            valueSize = 0;
        }
    }
}
