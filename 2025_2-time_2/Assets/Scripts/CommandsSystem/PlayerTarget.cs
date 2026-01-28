using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTarget : CommandTarget
{
    //This script is absolutely crap

    new protected void StartUp()
    {
        if (size == null)
            size = GetComponent<SizeEffect>();

        if (size != null)
        {
            size.Initialize(this, targetSize, sizeScriptable);
        }
    }

    new public void ChangeCurrentColor(string newColor) 
    {
        return;
    }

    new public string GetDisplayName()
    {
        return targetName;
    }
}
