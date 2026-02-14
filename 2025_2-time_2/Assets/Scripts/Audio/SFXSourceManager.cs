using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXSourceManager : MonoBehaviour
{
    [SerializeField] private AudioSource source;

    private void FixedUpdate()
    {
        if (!source.isPlaying) 
        {
            Destroy(gameObject);
        }
    }
}
