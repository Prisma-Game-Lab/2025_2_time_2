using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerConcussionDetection : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerController pc;

    [Header("Variables")]
    [SerializeField] private float minConcussionForce;

    [Header("Events")]
    [SerializeField] private UnityEvent OnConcussionEvent;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject colidedObject = collision.gameObject;
        Rigidbody2D targetRb;

        if (!colidedObject.CompareTag("Box")) 
        {
            if (!colidedObject.CompareTag("BoxMovementInhibitor"))
                return;

            colidedObject = colidedObject.transform.parent.gameObject;
            targetRb = colidedObject.GetComponent<Rigidbody2D>();
        }
        else 
        {
            targetRb = collision.rigidbody;
        }

        if (collision.GetContact(0).normal.y >= -0.5)
            return;

        CommandTarget targetScript = colidedObject.GetComponent<CommandTarget>();
        TargetSize targetSize = targetScript.GetTargetSize();

        //print(colidedObject.name);

        if (targetSize != TargetSize.Small)
        {
            float totalColisionEnergy = targetRb.velocity.y * targetRb.mass;
            //print(totalColisionEnergy);
            //totalColisionEnergy -= pc.rb.velocity.y * pc.rb.mass;

            if (totalColisionEnergy < minConcussionForce)
            {
                print("Concussion");
                OnConcussionEvent.Invoke();
            }
        }
    }
}
