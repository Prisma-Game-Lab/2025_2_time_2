using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandsSaver : MonoBehaviour
{
    private static List<CommandSavedData> savedData;

    private void OnEnable()
    {
        LevelManager.OnSceneChanged.AddListener(EraseData);
    }

    private void OnDisable()
    {
        LevelManager.OnSceneChanged.RemoveListener(EraseData);
    }

    public static void Save(List<CommandSavedData> data) 
    {
        savedData = data;
    }

    public static List<CommandSavedData> Load()
    {
        List<CommandSavedData> returnData = savedData;

        return returnData;
    }

    private void EraseData() 
    {
        savedData = null;
    }
}

public class CommandSavedData 
{
    public string slotText;
    public int targetIndex;
    public int modifierIndex;
}