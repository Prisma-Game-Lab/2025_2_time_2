using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpSaver : MonoBehaviour
{
    public static HelpSaver Instance { get; private set; }
    [HideInInspector] public List<CommandTooltipSO> knownCommands;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    
    public List<CommandTooltipSO> GetKnowCommands()
    {
        return knownCommands;
    }

    public void AddKnownCommand(CommandTooltipSO newCommand)
    {
        knownCommands.Add(newCommand);
    }
}
