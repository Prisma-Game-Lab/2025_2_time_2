using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/New Command")]
public class Command : ScriptableObject
{
    public string commandName;
    public CommandEffectType effect;
    public bool hasTarget;
    public Modifier[] modifiers;

    public bool HasModifiers() 
    {
        return modifiers.Length > 0;
    }

    public bool GetModifierValue(string modifierDisplay, out float modifierValue) 
    {
        foreach (Modifier modifier in modifiers)
        {
            if (modifier.displayName == modifierDisplay) 
            {
                modifierValue = modifier.modifierValue;
                return true;
            }
        }
        modifierValue = -1;
        return false;
    }
}

public enum CommandEffectType 
{
    Size, Clear
}

[System.Serializable]
public struct Modifier 
{
    public string displayName;
    public float modifierValue;
}
