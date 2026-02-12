using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject hintPanel;
    [SerializeField] private GameObject hudHolder;

    private void OnEnable()
    {
        GameManager.Instance.OnPause.AddListener(OnPause);
    }

    private void OnDisable()
    {
        GameManager.Instance.OnPause.RemoveListener(OnPause);
    }

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

    public void OnPause(bool state) 
    {
        SetHUD(!state);
    }

    public void SetHUD(bool state) 
    {
        hudHolder.SetActive(state);
    }
}
