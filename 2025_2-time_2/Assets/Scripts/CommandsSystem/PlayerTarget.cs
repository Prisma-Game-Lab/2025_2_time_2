using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTarget : CommandTarget
{
    //This script is absolutely crap

    protected override void StartUp()
    {
        if (size == null)
            size = GetComponent<SizeEffect>();

        if (size != null)
        {
            size.Initialize(this, targetSize);
        }
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
