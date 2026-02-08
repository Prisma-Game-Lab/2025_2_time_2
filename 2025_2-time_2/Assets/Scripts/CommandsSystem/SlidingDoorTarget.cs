using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoorTarget : CommandTarget
{
    protected override void StartUp()
    {
        return;
    }

    public override void ChangeCurrentColor(string newColor)
    {
        return;
    }

    public override string GetDisplayName()
    {
        return targetName;
    }
}
