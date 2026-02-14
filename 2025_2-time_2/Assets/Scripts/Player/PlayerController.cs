using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public Animator animator;

    [SerializeField] private PlayerState currentState = PlayerState.Idle;

    [Header("Events")]
    [SerializeField] private UnityEvent<PlayerState> OnPlayerStateChange;
    [SerializeField] private UnityEvent onDeathEvent;

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
        OnDeath();
    }

    public void OnDeath() 
    {
        onDeathEvent.Invoke();
        LevelManager.RestartLevel();
    }
}

public enum PlayerState
{
    Idle, Running, Jumping, Blocked
}