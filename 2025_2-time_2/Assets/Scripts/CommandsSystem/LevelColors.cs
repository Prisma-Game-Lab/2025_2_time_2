using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelColors : MonoBehaviour
{
    public static LevelColors instance;

    [SerializeField] private List<ColorInfo> availableColors;

    private void OnValidate()
    {
        if (instance == null)
            instance = this;   
    }

    private void Awake()
    {
        instance = this;
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }
    
    public ColorInfo GetTargetColor(string colorName) 
    {
        foreach (ColorInfo color in availableColors)
        {
            if (color.diplayName == colorName)
                return color;
        }
        return null;
    }

    public ColorInfo GetDefaultColor()
    {
        if (availableColors.Count == 0) 
            return null;
        return availableColors[0];
    }

    public List<string> GetAllTargetsColorName()
    {
        List<string> colorNames = new List<string>();
        foreach (ColorInfo color in availableColors) 
        {
            colorNames.Add(color.diplayName);
        }

        return colorNames;
    }
}
