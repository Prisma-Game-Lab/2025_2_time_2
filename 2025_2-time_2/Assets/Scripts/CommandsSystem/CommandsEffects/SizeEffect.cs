using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeEffect : CommandEffect
{
    private float scaleModifier;

    public void Start()
    {
        if (strength.Length < 3)
            Debug.LogError("Not Enought parameters for Strengh in size");

        switch (parameter.ToLower()) 
        {
            case "small":
                scaleModifier = strength[0];
                break;
            case "medium":
                scaleModifier = strength[1];
                break;
            case "big":
                scaleModifier = strength[2];
                break;
        }

        transform.localScale *= scaleModifier * modifier;
    }

    private void OnDestroy()
    {
        transform.localScale /= scaleModifier * modifier;
    }
}
