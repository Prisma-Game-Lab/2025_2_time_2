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

    [Header("Commands Variables")]
    [SerializeField] private int nSlots;
    [SerializeField] private float startTime;
    [SerializeField] private float timeBetweenCommands;

    [Header("Events")]
    [SerializeField] private UnityEvent OnRun = new UnityEvent();
    [SerializeField] private UnityEvent onValidCommands = new UnityEvent();
    [SerializeField] private UnityEvent onInvalidCommands = new UnityEvent();
    public static UnityEvent<CommandArguments> OnCommand = new UnityEvent<CommandArguments>();
    
    private Dictionary<string, CommandData> levelCommands;
    
    private bool running;
    private int validCommands;

    private void Start()
    {
        if (instance != null)
            Destroy(instance);
        instance = this;

        InitializeDictionary();
        consoleWindow.CreateSlots(nSlots);

        LoadCommands();
    }

    public void ToggleConsole() 
    {
        consoleWindow.ToggleWindow();
    }

    public void OpenConsole() 
    {
        consoleWindow.OpenWindow();
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

    private IEnumerator RunSequence()
    {
        yield return new WaitForSeconds(startTime);

        for (int i = 0; i < nSlots; i++) 
        {
            List<string> parameters;
            CommandData nextCommandData = consoleWindow.GetSlotData(i, out parameters);

            if (nextCommandData != null)
            {
                ActivateCommand(nextCommandData, parameters);
            }

            consoleWindow.BlockSlot(i);

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

    public void SetNValidCommands(int nValidCommands) 
    {
        if (validCommands == nValidCommands)
            return;

        validCommands = nValidCommands;

        if (validCommands == 0)
            onInvalidCommands.Invoke();
        else
            onValidCommands.Invoke();
    }

    private void LoadCommands() 
    {
        List<CommandSavedData> data = CommandsSaver.Load();

        if (data == null)
            return;

        consoleWindow.LoadSaveData(data);
    }

    public void SaveCommands()
    {
        List<CommandSavedData> data = consoleWindow.GetSaveData();

        CommandsSaver.Save(data);
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