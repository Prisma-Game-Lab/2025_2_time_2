using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private string StepSFXName;
    [SerializeField] private float StepSFXCooldown;

    private bool running = false;

    public void OnPlayerStateChange(PlayerState state)
    {
        running = false;

        switch (state)
        {
            case PlayerState.Running:
                running = true;
                StartCoroutine(StepSFXLoop());
                break;
        }
    }

    public IEnumerator StepSFXLoop() 
    {
        while (running)
        {
            AudioManager.Instance.PlaySFX(StepSFXName);
            yield return new WaitForSeconds(StepSFXCooldown);
        }
    }
}
