using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public Animator animator;

    public enum PlayerState
    {
        Idle, Running, Jumping, Blocked
    }
    [SerializeField] private PlayerState currentState = PlayerState.Idle;

    [Header("Events")]
    [SerializeField] private UnityEvent<PlayerState> OnPlayerStateChange;

    public void SetCurrentPlayerState(PlayerState newState) 
    {
        currentState = newState;
        OnPlayerStateChange.Invoke(currentState);
    }

    public PlayerState GetCurrentPlayerState() 
    {
        return currentState;
    }
    
    public void OnCrush() 
    {
        LevelManager.RestartLevel();
    }
}
