using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandsController : MonoBehaviour
{
    [SerializeField] private CommandData[] availableCommands;

    [Header("Commands Slots")]
    [SerializeField] private GameObject commandSlotPrefab;
    [SerializeField] private GameObject commandSlotHolder;
    [SerializeField] private int commandSlotsNum;
    [SerializeField] private float startTime;
    [SerializeField] private float timeBetweenCommands;
    private List<CMDController> commandsSlots = new List<CMDController>();

    private Dictionary<string, CommandData> levelCommands;
    private CommandEffect lastCommandEffectScript;
    private bool running;

    private PlayerController playerRef;

    private void Start()
    {
        playerRef = GameManager.Instance.GetPlayerRef().GetComponent<PlayerController>();
        playerRef.SetCurrentPlayerState(PlayerController.PlayerState.Blocked);
        InitializeDictionary();
        CreateSlots();
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

    public void RunCode() 
    {
        if (running)
            return;

        running = true;

        StartCoroutine(RunSequence());
    }

    private IEnumerator RunSequence()
    {
        playerRef.SetCurrentPlayerState(PlayerController.PlayerState.Idle);

        yield return new WaitForSeconds(startTime);

        foreach (var commandSlot in commandsSlots)
        {
            List<string> parameters;
            CommandData nextCommandData = commandSlot.GetData(out parameters);

            if (nextCommandData != null)
            {
                if (!ActivateCommand(nextCommandData, parameters))
                    print("errou");
            }

            yield return new WaitForSeconds(timeBetweenCommands);
        }
    }

    //public void OnCommandLine(string commandLine) 
    //{
    //    string[] commandKeyword = commandLine.ToLower().Split();

    //    foreach (CommandData command in availableCommands)
    //    { 
    //        if (command.commandScriptable.name.ToLower() == commandKeyword[0]) 
    //        {
    //            print("Activate: " + commandKeyword[0]);
    //            if (!ActivateCommand(command, commandKeyword))
    //                print("Incorrect Parameters");

    //            return;
    //        }
    //    }
    //    print("Incorrect Command");
    //}

    private void InitializeDictionary() 
    {
        levelCommands = new Dictionary<string, CommandData>();

        foreach (var command in availableCommands)
        {
            string commandName = command.commandScriptable.commandName;

            levelCommands.Add(commandName.ToLower(), command);
        }
    }

    private void CreateSlots() 
    {
        for (int i = 0; i < commandSlotsNum; i++)
        {
            GameObject newCommandSlot = Instantiate(commandSlotPrefab, commandSlotHolder.transform);
            CMDController newCMDController = newCommandSlot.GetComponent<CMDController>();
            commandsSlots.Add(newCMDController);
            newCMDController.SetCMDCReference(this);
        }
    }

    private bool ActivateCommand(CommandData data, List<string> parameters) 
    {
        int currentParameterIndex = 0;
        int nParameters = parameters.Count;

        GameObject target = gameObject;
        if (data.commandScriptable.hasTarget) 
        {
            if (nParameters <= currentParameterIndex) 
                return false;

            target = GetTarget(data, parameters[currentParameterIndex]);
            currentParameterIndex++;

            if (target == null) 
                return false;
        }


        float modifierValue = -1;
        if (data.commandScriptable.HasModifiers())
        {
            if (nParameters <= currentParameterIndex)
                return false;

            if (!data.commandScriptable.GetModifierValue(parameters[currentParameterIndex], out modifierValue))
                return false;

            currentParameterIndex++;
        }

        lastCommandEffectScript = InstanciateCommandScript(data.commandScriptable.effect, target);

        if (lastCommandEffectScript != null) 
        {
            if (data.commandScriptable.HasModifiers()) 
            {
                lastCommandEffectScript.SetModifier(modifierValue);
            }
            lastCommandEffectScript.Activate();
        }

        //if (data.commandScriptable.hasCustomStrength)
        //{
        //    lastCommandEffectScript.SetStrength(data.commandScriptable.customStrength);
        //}

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