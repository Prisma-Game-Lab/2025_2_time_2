using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]

public class ButtonScript : MonoBehaviour
{
    [Header("Button Settings")]
    [SerializeField] private float requiredMass = 1f;

    [Header("Visual & Platform")]
    
    [SerializeField] private Transform buttonTop;

    [SerializeField] private float pressDepth = 0.1f;   
    [SerializeField] private float pressSpeed = 5f;     

    [Header("Events")]
    public UnityEvent OnPressed;
    public UnityEvent OnReleased;

   
    private readonly List<Rigidbody2D> overlappingBodies = new List<Rigidbody2D>();

    private bool isPressed = false;   
    private float pressAmount = 0f;  

    private Vector3 topInitialLocalPos;
    private Vector3 topPressedLocalPos;

    public bool IsPressed => isPressed;

    private void Awake()
    {
        if (buttonTop == null)
            buttonTop = transform;
    }

    private void Start()
    {
        
        topInitialLocalPos = buttonTop.localPosition;
        topPressedLocalPos = topInitialLocalPos + Vector3.down * pressDepth;
    }

    private void Update()
    {
       
        Vector3 targetTop = Vector3.Lerp(topInitialLocalPos, topPressedLocalPos, pressAmount);
        buttonTop.localPosition = Vector3.MoveTowards(
            buttonTop.localPosition,
            targetTop,
            pressSpeed * Time.deltaTime
        );

        
        if (overlappingBodies.Count > 0)
        {
            RecalculateState();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var rb = other.attachedRigidbody;
        if (rb != null && !overlappingBodies.Contains(rb))
        {
            overlappingBodies.Add(rb);
            RecalculateState();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var rb = other.attachedRigidbody;
        if (rb != null && overlappingBodies.Contains(rb))
        {
            overlappingBodies.Remove(rb);
            RecalculateState();
        }
    }
    
    private void RecalculateState()
    {
        float totalMass = 0f;

        for (int i = overlappingBodies.Count - 1; i >= 0; i--)
        {
            if (overlappingBodies[i] == null)
            {
                overlappingBodies.RemoveAt(i);
                continue;
            }

            totalMass += overlappingBodies[i].mass;
        }

        
        float targetAmount = requiredMass > 0f
            ? Mathf.Clamp01(totalMass / requiredMass)
            : 1f;

        pressAmount = targetAmount;

        bool shouldBePressed = pressAmount >= 1f;

        if (shouldBePressed != isPressed)
        {
            isPressed = shouldBePressed;

            if (isPressed)
                OnPressed?.Invoke();
            else
                OnReleased?.Invoke();
        }
    }
}
