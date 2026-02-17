using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private string StepSFXName;
    [SerializeField] private float StepSFXCooldown;
    [SerializeField] private string DeathSFXName;

    private bool running = false;
    private bool looping = false;

    public void OnPlayerStateChange(PlayerState state)
    {
        running = false;

        switch (state)
        {
            case PlayerState.Running:
                running = true;
                if (!looping)
                    StartCoroutine(StepSFXLoop());
                break;
        }
    }

    public IEnumerator StepSFXLoop() 
    {
        looping = true;
        while (running)
        {
            AudioManager.Instance.PlaySFX(StepSFXName);
            yield return new WaitForSeconds(StepSFXCooldown);
        }
        looping = false;
    }

    public void PlayDeathSFX() 
    {
        AudioManager.Instance.PlaySFX(DeathSFXName);
    }
}
