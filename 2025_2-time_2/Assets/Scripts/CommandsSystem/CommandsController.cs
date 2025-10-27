using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandsController : MonoBehaviour
{
    [SerializeField] private CommandData[] availableCommands;
    [SerializeField] private Dictionary<string, CommandData> levelCommands;
    private CommandEffect lastCommandEffectScript;

    private void Start()
    {
        InitializeDictionary();
    }

    private void InitializeDictionary() 
    {
        levelCommands = new Dictionary<string, CommandData>();

        foreach (var command in availableCommands)
        {
            string commandName = command.commandScriptable.commandName;

            levelCommands.Add(commandName.ToLower(), command);
        }
    }

    public CommandData CheckCommand(string commandName) 
    {
        CommandData currentComand;

        if (!levelCommands.TryGetValue(commandName, out currentComand)) 
        {
            return null;
        }

        return currentComand;
    }

    public void OnCommandLine(string commandLine) 
    {
        string[] commandKeyword = commandLine.ToLower().Split();

        foreach (CommandData command in availableCommands)
        { 
            if (command.commandScriptable.name.ToLower() == commandKeyword[0]) 
            {
                print("Activate: " + commandKeyword[0]);
                if (!ActivateCommand(command, commandKeyword))
                    print("Incorrect Parameters");

                return;
            }
        }
        print("Incorrect Command");
    }

    private bool ActivateCommand(CommandData data, string[] parameters) 
    {
        GameObject target = gameObject;
        int currentParameterIndex = 1;

        print(parameters.Length);

        if (data.commandScriptable.hasTarget) 
        {
            if (parameters.Length <= currentParameterIndex) 
                return false;

            target = GetTarget(data, parameters[currentParameterIndex]);
            currentParameterIndex++;

            if (target == null) 
                return false;
        }

        if (data.commandScriptable.HasModifiers())
        {
            if (parameters.Length <= currentParameterIndex)
                return false;
        }

        if (lastCommandEffectScript != null)
            Destroy(lastCommandEffectScript);
        lastCommandEffectScript = InstanciateCommandScript(data.commandScriptable.effect, target);

        if (data.commandScriptable.HasModifiers()) 
        {
            lastCommandEffectScript.SetParameter(parameters[currentParameterIndex]);
        }

        //if (data.commandScriptable.hasCustomStrength)
        //{
        //    lastCommandEffectScript.SetStrength(data.commandScriptable.customStrength);
        //}

        lastCommandEffectScript.SetModifier(data.commandModifier);

        if (lastCommandEffectScript != null)
            lastCommandEffectScript.Activate();

        return true;
    }

    private GameObject GetTarget(CommandData data, string targetName) 
    {
        targetName = targetName.ToLower();

        foreach (Target target in data.sceneTargets)
        {
            if (targetName == target.displayName.ToLower()) 
            {
                return target.targetGameObject;
            }
        }

        return null;
    }

    private CommandEffect InstanciateCommandScript(CommandEffectType effect, GameObject target) 
    {
        switch (effect)
        {
            case CommandEffectType.Size:
                return target.AddComponent<SizeEffect>();
            case CommandEffectType.Clear:
                return null;
        }

        return null;
    }
}

[System.Serializable]
public class CommandData 
{
    public Command commandScriptable;
    public float commandModifier;
    public Target[] sceneTargets;
}

[System.Serializable]
public struct Target 
{
    public string displayName;
    public GameObject targetGameObject;
}