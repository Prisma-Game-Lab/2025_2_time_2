using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject hintPanel;

    public void OnConsoleButton() 
    {
        CommandsController.instance.ToggleConsole();
    }

    public void OnHelpButton() 
    {
        hintPanel.SetActive(!hintPanel.activeInHierarchy);
    }

    public void OnRestartButton() 
    {
        LevelManager.RestartLevel();
    }
}
