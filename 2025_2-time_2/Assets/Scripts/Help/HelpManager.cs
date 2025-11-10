using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;

public class HelpManager : MonoBehaviour
{

    [SerializeField] private List<CommandTooltipSO> commands;

    [SerializeField] private List<CommandTooltipSO> knownCommands;

    [SerializeField] private List<TextMeshProUGUI> tooltipNames;

    [SerializeField] private List<TextMeshProUGUI> tooltipDesc;

    private int currentTooltipIndex = 0;


    void Start()
    {
        if (knownCommands.Count > 0)
        {
            DisplayKnowCommands();
        }
    }

    public void AddCommand(string commandName)
    {
        foreach (CommandTooltipSO command in commands)
        {
            if (commandName == command.commandName)
            {
                if (knownCommands.Contains(command))
                {
                    Debug.Log("Command is Already Known");
                }
                else
                {
                    DisplayCommand(command);
                }
            }
            else
            {
                Debug.Log("Command Tooltip Not Found :(");
            }
        }
    }
    
    public void DisplayCommand(CommandTooltipSO command)
    {
        tooltipNames[currentTooltipIndex].text = command.commandName;
        tooltipDesc[currentTooltipIndex].text = command.commandDesc;

        currentTooltipIndex++;

        if(!knownCommands.Contains(command))
        knownCommands.Add(command);
    }
    
    public void DisplayKnowCommands()
    {
        foreach (CommandTooltipSO command in knownCommands)
        {
            DisplayCommand(command);
        }
    }
    
}
