using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CMDController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_InputField cmdInputField;
    [SerializeField] private GameObject targetHolder;
    [SerializeField] private TMP_Dropdown targetDropdown;
    [SerializeField] private GameObject modifierHolder;
    [SerializeField] private TMP_Dropdown modifierDropdown;
    private CommandsController cmdController;

    [Header("Variables")]
    [SerializeField] private Color validCommandColor;

    [Header("Events")]
    [SerializeField] private UnityEvent<string> OnCommandLine;

    [Header("Reference to Pause Menu")]

    [SerializeField] private PauseMenu pauseMenu;

    private CommandData currentStoredCommand;
    private bool selected;
    private Color originalColor;

    private HelpManager helpManager;

    private void Start()
    {
        originalColor = cmdInputField.textComponent.color;
        helpManager = FindObjectOfType<HelpManager>();
    }

    public void SetCMDCReference(CommandsController cmd) 
    {
        cmdController = cmd;
    }

    private void CheckCommand(string commandName) 
    {
        currentStoredCommand = cmdController.CheckCommand(commandName);

        if (currentStoredCommand == null)
        {
            cmdInputField.textComponent.color = originalColor;
            targetHolder.SetActive(false);
            modifierHolder.SetActive(false);
            currentStoredCommand = null;
            return;
        }

        // Registers command
        if (helpManager != null) 
            helpManager.AddCommand(commandName);
        
        cmdInputField.textComponent.color = validCommandColor;
        cmdInputField.SetTextWithoutNotify(currentStoredCommand.commandScriptable.commandName);

        SetTargetDropdown(currentStoredCommand);
        SetModifierDropdown(currentStoredCommand);
    }

    private void SetTargetDropdown(CommandData commandData) 
    {
        Command command = commandData.commandScriptable;

        targetDropdown.ClearOptions();
        if (!command.hasTarget) 
        {
            targetHolder.SetActive(false);
        }
        else 
        {
            targetHolder.SetActive(true);

            List<string> targetNames = new List<string>();
            foreach (CommandTarget targetObject in commandData.sceneTargets) 
            {
                string targetName = targetObject.GetDisplayName();
                if (!targetNames.Contains(targetName))
                    targetNames.Add(targetName);
            }
            targetDropdown.AddOptions(targetNames);
        }
    }

    private void SetModifierDropdown(CommandData commandData)
    {
        Command command = commandData.commandScriptable;

        modifierDropdown.ClearOptions();
        if (command.effect == CommandEffectType.Color) 
        {
            modifierHolder.SetActive(true);

            modifierDropdown.AddOptions(LevelColors.instance.GetAllTargetsColorName());
        }
        else if (!command.HasModifiers())
        {
            modifierHolder.SetActive(false);
        }
        else
        {
            modifierHolder.SetActive(true);

            List<string> modifiersKeyword = new List<string>();
            foreach (Modifier modifier in command.modifiers)
            {
                modifiersKeyword.Add(modifier.displayName);
            }
            modifierDropdown.AddOptions(modifiersKeyword);
        }
    }

    //public void OnCommandLineEnter() 
    //{
    //    if (!selected) return;

    //    cmdController.OnCommandLine(cmdInputField.text);
    //    OnCommandLine.Invoke(cmdInputField.text);
    //    cmdInputField.text = string.Empty;
    //}

    //public void OnEnterAction(InputAction.CallbackContext action)
    //{
    //    if (action.performed)
    //    {
    //        if (!selected) return;

    //        OnCommandLineEnter();
    //    }
    //}

     public void OnPauseButton(InputAction.CallbackContext inputValue)
    {
        if (inputValue.performed)
        {
            pauseMenu.TogglePause();
        }
    }

    public void OnSelected()
    {
        selected = true;
    }

    public void OnDeselected()
    {
        selected = false;
        CheckCommand(cmdInputField.text.ToLower().Trim(' '));
    }

    public CommandData GetData(out List<string> parameters) 
    {
        parameters = new List<string>();

        if (currentStoredCommand == null)
        {
            return null;
        }

        Command commandScriptable = currentStoredCommand.commandScriptable;

        if (commandScriptable.hasTarget) 
        {
            string targetName = targetDropdown.options[targetDropdown.value].text;
            parameters.Add(targetName);
        }

        if (commandScriptable.HasModifiers()) 
        {
            string modifier = modifierDropdown.options[modifierDropdown.value].text;
            parameters.Add(modifier);
        }

        return currentStoredCommand;
    }
}
