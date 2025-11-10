using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "ScriptableObjects/New Command Tooltip") ]
public class CommandTooltipSO : ScriptableObject
{
    [Header("Command Name")]
    public string commandName;
    [Header("Command Description")]
    public string commandDesc;
}
