using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTriggerEnterEvent : MonoBehaviour
{
    [SerializeField] private UnityEvent customEvent;
    [SerializeField] private bool oneTime;

    private bool triggered;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (oneTime && triggered)
        {
            return;
        }

        customEvent.Invoke();
        triggered = true;
    }
}
