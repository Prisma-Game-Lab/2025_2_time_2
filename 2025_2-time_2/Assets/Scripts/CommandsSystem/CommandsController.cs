using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandsController : MonoBehaviour
{
    [SerializeField] private CommandData[] availableCommands;
    private CommandEffect lastCommandEffectScript;

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

        if (data.commandScriptable.hasParameter)
        {
            if (parameters.Length <= currentParameterIndex)
                return false;
        }

        if (lastCommandEffectScript != null)
            Destroy(lastCommandEffectScript);
        lastCommandEffectScript = InstanciateCommandScript(data.commandScriptable.effect, target);

        if (data.commandScriptable.hasParameter) 
        {
            lastCommandEffectScript.SetParameter(parameters[currentParameterIndex]);
        }

        if (data.commandScriptable.hasCustomStrength)
        {
            lastCommandEffectScript.SetStrength(data.commandScriptable.customStrength);
        }

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
struct CommandData 
{
    public Command commandScriptable;
    public float commandModifier;
    public Target[] sceneTargets;
}

[System.Serializable]
struct Target 
{
    public string displayName;
    public GameObject targetGameObject;
}