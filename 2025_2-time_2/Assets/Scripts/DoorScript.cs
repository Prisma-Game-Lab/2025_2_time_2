using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        SceneController.instance.NextLevel();
    }
}
