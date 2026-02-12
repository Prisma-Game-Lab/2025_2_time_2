using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("Tabs")]
    [SerializeField] private GameObject PauseHolder;
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject PausePanel;
    [SerializeField] private GameObject Controls;
    [SerializeField] private GameObject Audio;

    private void Start()
    {
        canvas.worldCamera = Camera.main;
        canvas.sortingLayerName = "UI";
    }

    public void TogglePause()
    {
        bool isActive = PauseHolder.activeSelf;

        PauseHolder.SetActive(!isActive);
        PausePanel.SetActive(!isActive);
        Controls.SetActive(false);
        Audio.SetActive(false);
        GameManager.Instance.SetPause(!isActive);
    }

    public void ToMenu()
    {
        TogglePause();
        LevelManager.LoadSceneByName("MainMenu");
    }

    public void OnPauseKey(InputAction.CallbackContext context) 
    {
        if (context.performed)
            TogglePause();
    }

    public void OnButtonHover()
    {
        AudioManager.Instance.PlaySFX("ButtonHover");
    }
}