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
