using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().OnDeath();
        }
    }
}
