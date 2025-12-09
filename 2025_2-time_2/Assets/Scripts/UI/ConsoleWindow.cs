using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleWindow : WindowController
{
    [Header("References")]
    [SerializeField] private CommandsController cmdController;
    [SerializeField] private Button runButton;
    [SerializeField] private Button clearButton;

    [Header("Commands Slots")]
    [SerializeField] private GameObject commandSlotPrefab;
    [SerializeField] private GameObject commandSlotHolder;

    private List<CMDController> commandsSlots = new List<CMDController>();

    int nValidSlots = 0;
    int nEmptySlots = 0;

    public void CreateSlots(int nSlots)
    {
        for (int i = 0; i < nSlots; i++)
        {
            GameObject newCommandSlot = Instantiate(commandSlotPrefab, commandSlotHolder.transform);
            CMDController newCMDController = newCommandSlot.GetComponent<CMDController>();
            commandsSlots.Add(newCMDController);
            newCMDController.SetWindowReference(this);
        }

        nEmptySlots = commandsSlots.Count;
    }

    public CommandData GetSlotData(int index, out List<string> parameters)
    {
        CommandData commandData = commandsSlots[index].GetData(out parameters);

        return commandData;
    }

    public void BlockSlot(int index)
    {
        commandsSlots[index].BlockInput();
    }

    public CommandData ValidateCommand(string commandName, ref bool empty, ref bool valid)
    {
        string cleanCommandName = commandName.ToLower().Trim(' ');
        CommandData data = cmdController.CheckCommand(cleanCommandName);

        AlterSlotCounter(commandName == "", ref empty, ref nEmptySlots);
        AlterSlotCounter(data != null, ref valid, ref nValidSlots);

        SetButtons();

        cmdController.SetNValidCommands(nValidSlots);

        return data;
    }

    private void AlterSlotCounter(bool condition, ref bool previousCondition, ref int counter) 
    {
        if (condition)
        {
            if (!previousCondition)
            {
                counter++;
            }

            previousCondition = true;
        }
        else
        {
            if (previousCondition)
            {
                counter--;
            }

            previousCondition = false;
        }
    }

    public void OnRun() 
    {
        runButton.interactable = false;
        clearButton.interactable = false;
        CloseWindow();
    }

    public void OnRunButtonPress()
    {
        cmdController.RunCode();
    }

    public void OnClearButtonPress()
    {
        foreach (CMDController slot in commandsSlots) 
        {
            slot.Clear();
        }

        nEmptySlots = commandsSlots.Count;
        nValidSlots = 0;

        SetButtons();
    }

    private void SetButtons() 
    {
        if (nEmptySlots < commandsSlots.Count)
        {
            //Enable Clear Button
            clearButton.interactable = true;
        }
        else
        {
            //Disable Clear Button
            clearButton.interactable = false;
        }

        if (nValidSlots > 0)
        {
            //Enable Run Button
            runButton.interactable = true;
        }
        else
        {
            //Disable Run Button
            runButton.interactable = false;
        }
    }
}
