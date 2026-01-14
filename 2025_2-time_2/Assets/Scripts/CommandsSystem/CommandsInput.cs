using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CommandsInput : MonoBehaviour
{
    [SerializeField] private UnityEvent OnConsoleButtonEvent;

    public void OnConsoleButton(InputAction.CallbackContext context) 
    {
        if (context.performed) 
        {
            OnConsoleButtonEvent.Invoke();
        }
    }
}
