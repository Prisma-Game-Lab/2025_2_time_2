using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem jumpDustParticle;
    [SerializeField] private ParticleSystem runDustParticle;

    public void OnPlayerStateChange(PlayerState newState) 
    {
        switch (newState)
        {
            case PlayerState.Idle:
                OnIdle();
                break;
            case PlayerState.Running:
                OnRun();
                break;
            case PlayerState.Jumping:
                OnJump();
                break;
            case PlayerState.Blocked:
                OnBlocked();
                break;
        }
    }

    private void OnIdle() 
    {
        runDustParticle.Stop();
    }

    private void OnRun() 
    {
        runDustParticle.Play();
    }

    private void OnJump() 
    {
        runDustParticle.Stop();
        jumpDustParticle.Play();
    }

    private void OnBlocked()
    {
        runDustParticle.Stop();
    }
}
