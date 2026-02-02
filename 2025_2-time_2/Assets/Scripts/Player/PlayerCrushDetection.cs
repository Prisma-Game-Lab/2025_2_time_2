using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCrushDetection : MonoBehaviour
{
    [Header("Crush Variables")]
    [SerializeField] private int maxCollisionFrames;

    [Header("Events")]
    [SerializeField] private UnityEvent OnCrushEvent;

    private int nCollision = 0;
    private int framesInCollision = 0;
    private bool triggered;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        nCollision++;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        nCollision--;

        if (nCollision == 0)
        {
            framesInCollision = 0;
        }
    }

    private void FixedUpdate()
    {
        if (nCollision > 0) 
        {
            framesInCollision++;
            if (framesInCollision >= maxCollisionFrames) 
            {
                print("Crushed");
                OnCrushEvent.Invoke();
                framesInCollision = 0;
            }
        }
    }
}
