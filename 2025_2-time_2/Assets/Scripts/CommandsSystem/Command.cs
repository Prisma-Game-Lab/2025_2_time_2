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
    public float strength;

    public bool HasModifiers() 
    {
        return modifiers.Length > 0 || effect == CommandEffectType.Color;
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

    public bool GetModifierValue(string modifierDisplay, out float modifierValue, out int index)
    {
        index = 0;
        foreach (Modifier modifier in modifiers)
        {
            if (modifier.displayName == modifierDisplay)
            {
                modifierValue = modifier.modifierValue;
                return true;
            }
            index++;
        }
        index = -1;
        modifierValue = -1;

        return false;
    }

    public bool GetModifierValue(int index, out float modifierValue)
    {
        if (modifiers.Length < index)
        {
            modifierValue = -1;
            return false;
        }

        modifierValue = modifiers[index].modifierValue;
        return true;
    }
}

public enum CommandEffectType 
{
    Size, Clear, Color, Stop, Swap
}

[System.Serializable]
public struct Modifier 
{
    public string displayName;
    public float modifierValue;
}
