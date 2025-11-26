using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleWindow : WindowController
{
    [Header("References")]
    [SerializeField] private CommandsController cmdController;
    [SerializeField] private Button runButton;

    public void OnRun() 
    {
        runButton.interactable = false;
        CloseWindow();
    }

    public void EnableRunButton()
    {
        runButton.interactable = true;
    }

    public void DisableRunButton()
    {
        runButton.interactable = false;
    }
}
