using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem jumpDustParticle;

    public void OnPlayerStateChange(PlayerState newState) 
    {
        switch (newState)
        {
            case PlayerState.Jumping:
                OnJump();
                break;
        }
    }

    private void OnJump() 
    {
        jumpDustParticle.Play();
    }
}
