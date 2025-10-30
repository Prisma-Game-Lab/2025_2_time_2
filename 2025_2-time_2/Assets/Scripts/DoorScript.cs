using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    private bool triggered;
    [SerializeField] private string nextSceneName; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!triggered) 
        {
            LevelManager.LoadSceneByName(nextSceneName);
            PlayerController pc = collision.GetComponent<PlayerController>();
            pc.SetCurrentPlayerState(PlayerController.PlayerState.Blocked);
            pc.rb.velocity = Vector2.up * pc.rb.velocity;
            triggered = true;
        }
    }
}
