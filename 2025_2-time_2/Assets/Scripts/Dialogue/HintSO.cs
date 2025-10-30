using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "ScriptableObjects/New Hint") ]
public class HintSO : ScriptableObject
{
    [Header("Hint Label")]
    public string hintLabel;
    [Header("Hint Text")]
    public string[] dialogue;
}
