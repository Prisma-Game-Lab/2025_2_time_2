using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private PlayerController pc;

    public void OnStateChange(PlayerController.PlayerState playerState) 
    {
        bool idle = false;
        bool running = false;
        bool jumping = false;
        bool blocked = false;

        switch (playerState) 
        {
            case PlayerController.PlayerState.Idle:
                idle = true;
                break;
            case PlayerController.PlayerState.Running:
                running = true;
                break;
            case PlayerController.PlayerState.Jumping:
                jumping = true;
                break;
            case PlayerController.PlayerState.Blocked:
                blocked = true;
                break;
        }

        pc.animator.SetBool("Idle", idle);
        pc.animator.SetBool("Running", running);
        pc.animator.SetBool("Jumping", jumping);
        pc.animator.SetBool("Blocked", blocked);
    }
}
