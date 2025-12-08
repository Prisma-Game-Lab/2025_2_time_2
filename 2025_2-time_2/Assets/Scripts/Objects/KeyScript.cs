using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyScript : MonoBehaviour
{
    public static UnityEvent OnKeyPickup = new UnityEvent();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnKeyPickup?.Invoke();

        Destroy(gameObject);
    }
}
