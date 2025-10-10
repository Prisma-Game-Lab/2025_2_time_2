using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CMDController : MonoBehaviour
{
    [SerializeField] private TMP_InputField cmdInputField;
    [SerializeField] private UnityEvent<string> OnCommandLine;

    private bool selected;

    public void OnCommandLineEnter() 
    {
        if (!selected) return;

        OnCommandLine.Invoke(cmdInputField.text);
        cmdInputField.text = string.Empty;
    }

    public void OnEnterAction(InputAction.CallbackContext action) 
    {
        if (action.performed) 
        {
            if (!selected) return;

            OnCommandLineEnter();
        }
    }

    public void OnSelected()
    {
        selected = true;
    }

    public void OnDeselected()
    {
        selected = false;
    }
}
