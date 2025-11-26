using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CommandsController : MonoBehaviour
{
    public static CommandsController instance;
    [Header("References")]
    [SerializeField] private ConsoleWindow consoleWindow;

    [Header("Available Commands List")]
    [SerializeField] private CommandData[] availableCommands;

    [Header("Commands Slots")]
    [SerializeField] private GameObject commandSlotPrefab;
    [SerializeField] private GameObject commandSlotHolder;
    [SerializeField] private int commandSlotsNum;
    [SerializeField] private float startTime;
    [SerializeField] private float timeBetweenCommands;

    [Header("Events")]
    [SerializeField] private UnityEvent OnRun = new UnityEvent();
    [SerializeField] private UnityEvent onValidCommands = new UnityEvent();
    [SerializeField] private UnityEvent onInvalidCommands = new UnityEvent();
    public static UnityEvent<CommandArguments> OnCommand = new UnityEvent<CommandArguments>();
    
    private List<CMDController> commandsSlots = new List<CMDController>();
    private Dictionary<string, CommandData> levelCommands;

    private PlayerController playerRef;
    
    private bool running;
    private int validCommands;

    private void Start()
    {
        if (instance != null)
            Destroy(instance);
        instance = this;

        playerRef = GameManager.Instance.GetPlayerRef().GetComponent<PlayerController>();
        playerRef.SetCurrentPlayerState(PlayerController.PlayerState.Blocked);
        InitializeDictionary();
        CreateSlots();
    }

    public void ToggleConsole() 
    {
        consoleWindow.ToggleWindow();
    }

    public CommandData CheckCommand(string commandName, bool storedValidCommand) 
    {
        CommandData currentComand;

        if (!levelCommands.TryGetValue(commandName, out currentComand)) 
        {
            if (storedValidCommand) 
            {
                validCommands--;
                if (validCommands == 0)
                    onInvalidCommands.Invoke();
            }

            return null;
        }

        if (!storedValidCommand)
        {
            if (validCommands == 0)
                onValidCommands.Invoke();
            validCommands++;
        }

        return currentComand;
    }

    public void RunCode() 
    {
        if (running || validCommands == 0)
            return;

        OnRun.Invoke();

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

            commandSlot.BlockInput();

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