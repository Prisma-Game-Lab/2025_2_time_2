using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private PlayerController pc;

    public void OnStateChange(PlayerState playerState) 
    {
        bool idle = false;
        bool running = false;
        bool jumping = false;
        bool blocked = false;

        switch (playerState) 
        {
            case PlayerState.Idle:
                idle = true;
                break;
            case PlayerState.Running:
                running = true;
                break;
            case PlayerState.Jumping:
                jumping = true;
                break;
            case PlayerState.Blocked:
                blocked = true;
                break;
        }

        pc.animator.SetBool("Idle", idle);
        pc.animator.SetBool("Running", running);
        pc.animator.SetBool("Jumping", jumping);
        pc.animator.SetBool("Blocked", blocked);
    }
}
