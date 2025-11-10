using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CommandsController : MonoBehaviour
{
    [SerializeField] private CommandData[] availableCommands;

    [Header("Commands Slots")]
    [SerializeField] private GameObject commandSlotPrefab;
    [SerializeField] private GameObject commandSlotHolder;
    [SerializeField] private int commandSlotsNum;
    [SerializeField] private float startTime;
    [SerializeField] private float timeBetweenCommands;

    [Header("Events")]
    public static UnityEvent<CommandArguments> OnCommand = new UnityEvent<CommandArguments>();
    
    private List<CMDController> commandsSlots = new List<CMDController>();
    private Dictionary<string, CommandData> levelCommands;

    private PlayerController playerRef;
    
    private bool running;


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
                ActivateCommand(nextCommandData, parameters);
            }

            yield return new WaitForSeconds(timeBetweenCommands);
        }
    }

    private void ActivateCommand(CommandData data, List<string> parameters) 
    {
        CommandArguments commandArguments = new CommandArguments();

        commandArguments.commandScriptable = data.commandScriptable;
        commandArguments.parameters = parameters;

        OnCommand.Invoke(commandArguments);
    }
}

[System.Serializable]
public class CommandData 
{
    public Command commandScriptable;
    public float commandModifier;
    public CommandTarget[] sceneTargets;
}

public class CommandArguments
{
    public Command commandScriptable;
    public List<string> parameters;
}