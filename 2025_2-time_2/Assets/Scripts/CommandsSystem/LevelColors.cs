using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelColors : MonoBehaviour
{
    public static LevelColors instance;

    [SerializeField] private List<TargetColor> availableColors;

    private void OnValidate()
    {
        if (instance == null)
            instance = this;   
    }

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(instance);
        instance = this;
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }
    
    public TargetColor GetTargetColor(string colorName) 
    {
        foreach (TargetColor color in availableColors)
        {
            if (color.diplayName == colorName)
                return color;
        }
        return null;
    }

    public List<string> GetAllTargetsColorName()
    {
        List<string> colorNames = new List<string>();
        foreach (TargetColor color in availableColors) 
        {
            colorNames.Add(color.diplayName);
        }

        return colorNames;
    }
}

[System.Serializable]
public class TargetColor 
{
    public string diplayName;
    public Color RGB;
}
