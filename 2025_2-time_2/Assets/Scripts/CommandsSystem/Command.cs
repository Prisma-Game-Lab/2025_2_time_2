using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/New Command")]
public class Command : ScriptableObject
{
    public string commandName;
    public CommandEffectType effect;
    public bool hasTarget;
    public bool hasParameter;
}

public enum CommandEffectType 
{
    Size
}
