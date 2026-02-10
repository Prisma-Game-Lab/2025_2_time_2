using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("Tabs")]
    [SerializeField] private GameObject PausePanel;
    [SerializeField] private GameObject Controls;
    [SerializeField] private GameObject Audio;

    [SerializeField] private GameObject Background;

    public void TogglePause()
    {
        bool isActive = Background.activeSelf;

        Background.SetActive(!isActive);
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
}