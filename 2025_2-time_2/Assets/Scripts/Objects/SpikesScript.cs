using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesScript : MonoBehaviour
{

[SerializeField] private HUDController hc;
private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        hc.OnRestartButton();
    }

}
