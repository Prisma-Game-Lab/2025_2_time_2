using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Configuration")]
    [SerializeField] private bool locked;
    [SerializeField] private bool unlockNextLevel;
    [SerializeField] private string nextSceneName;
    [SerializeField] private Sprite lockedSprite;
    [SerializeField] private Sprite unlockedSprite;
    [SerializeField] private ParticleSystem unlockedParticles;

    private bool opened;

    private void Start()
    {
        Initialization();

        if (animator != null) 
        {
            animator.SetBool("Locked", locked);
            animator.SetTrigger("InstaLock");
        }

        if (!locked) 
        {
            unlockedParticles.Play();
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

    public void Open(Collider2D collision) 
    {
        if (locked || opened)
            return;

        opened = true;

        if (unlockNextLevel)
            LevelManager.UnlockNextLevel();

        LevelManager.LoadSceneByName(nextSceneName);
        PlayerController pc = collision.GetComponent<PlayerController>();
        pc.SetCurrentPlayerState(PlayerState.Blocked);
        pc.rb.velocity = Vector2.up * pc.rb.velocity;
    }

    public void SetLockState(bool locked) 
    {
        this.locked = locked;
        animator.SetBool("Locked", locked);
    }

    private void Unlock() 
    {
        SetLockState(false);
        unlockedParticles.Play();
    }
}
