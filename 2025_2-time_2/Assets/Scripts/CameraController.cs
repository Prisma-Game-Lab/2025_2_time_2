using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;

    [SerializeField] private float smoothTime = .20f;

    private Vector3 velocity = Vector3.zero;
    
    void LateUpdate()
    {
        Vector3 pos = player.position; 

        pos.z = transform.position.z;

        transform.position = Vector3.SmoothDamp (transform.position, pos, ref velocity, smoothTime);

    }
}
