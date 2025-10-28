using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private float jumpBufferAmount;

    [Header("Events")]
    [SerializeField] private UnityEvent<float> OnMoveEvent;
    [SerializeField] private UnityEvent<bool> OnJumpActionEvent;
    [SerializeField] private UnityEvent<bool> OnJumpBufferEvent;

    private bool jumpOnBuffer;
    private float currentJumpBuffer;

    private void FixedUpdate()
    {
        if (jumpOnBuffer) 
        {
            if (currentJumpBuffer > 0) 
            {
                currentJumpBuffer -= Time.deltaTime;
            }
            else 
            {
                jumpOnBuffer = false;
                OnJumpBufferEvent.Invoke(false);
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context) 
    {
        float moveInput = context.ReadValue<float>();

        OnMoveEvent.Invoke(moveInput);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnJumpActionEvent.Invoke(true);
            OnJumpBufferEvent.Invoke(true);
            currentJumpBuffer = jumpBufferAmount;
            jumpOnBuffer = true;
        }
        else if (context.canceled) 
        {
            OnJumpActionEvent.Invoke(false);
        }
    }
}
