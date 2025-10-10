using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeEffect : CommandEffect
{
    private Vector3 startScale;

    public void Start()
    {
        startScale = transform.localScale;
        if (parameter.ToLower() == "big")
            transform.localScale *= 2;
    }

    private void OnDestroy()
    {
        transform.localScale = startScale;
    }
}
