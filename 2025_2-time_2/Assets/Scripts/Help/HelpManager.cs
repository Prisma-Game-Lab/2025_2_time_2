using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HelpManager : MonoBehaviour
{

    [SerializeField] private List<CommandTooltipSO> commands;

    [SerializeField] private List<CommandTooltipSO> newCommands;
     private List<CommandTooltipSO> knownCommands;

    [SerializeField] private List<TextMeshProUGUI> tooltipNames;

    [SerializeField] private List<TextMeshProUGUI> tooltipDesc;

    private int currentTooltipIndex = 0;

    private HelpSaver hs;


    void Start()
    {
        hs = HelpSaver.Instance;
        knownCommands = hs.GetKnowCommands();

        AddNewCommands();

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
                    hs.AddKnownCommand(command);
                    DisplayCommand(command);
                }
            }
            
        }
    }
    
    public void DisplayCommand(CommandTooltipSO command)
    {
        if (currentTooltipIndex >= tooltipNames.Count)
            return;

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

    public void AddNewCommands()
    {
        foreach (CommandTooltipSO command in newCommands)
        {
            if (!knownCommands.Contains(command))
            knownCommands.Add(command);
        }
    }
    
}
