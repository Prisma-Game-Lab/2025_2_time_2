using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Configuration")]
    [SerializeField] private string nextSceneName;
    [SerializeField] private bool locked;
    [SerializeField] private Sprite lockedSprite;
    [SerializeField] private Sprite unlockedSprite;

    private bool triggered;

    private void Start()
    {
        Initialization();

        if (animator != null) 
        {
            animator.SetBool("Locked", locked);
            animator.SetTrigger("InstaLock");
        }
    }

    private void OnValidate()
    {
        Initialization();

        if (spriteRenderer == null)
            return;

        if (locked)
            spriteRenderer.sprite = lockedSprite;
        else
            spriteRenderer.sprite = unlockedSprite;
    }

    private void Initialization()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        KeyScript.OnKeyPickup.AddListener(Unlock);
    }

    private void OnDisable()
    {
        KeyScript.OnKeyPickup.RemoveListener(Unlock);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (locked)
            return;
        if (triggered)
            return;

        LevelManager.LoadSceneByName(nextSceneName);
        PlayerController pc = collision.GetComponent<PlayerController>();
        pc.SetCurrentPlayerState(PlayerController.PlayerState.Blocked);
        pc.rb.velocity = Vector2.up * pc.rb.velocity;
        triggered = true;
    }

    public void SetLockState(bool locked) 
    {
        this.locked = locked;
        animator.SetBool("Locked", locked);
    }

    private void Unlock() 
    {
        SetLockState(false);
    }
}
