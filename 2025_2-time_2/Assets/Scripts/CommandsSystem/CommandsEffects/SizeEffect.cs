using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeEffect : CommandEffect
{
    private float scaleModifier;

    private Vector2 originalScale;
    private Vector2 targetScale;

    private bool interpolateScale;
    private float currentTime;
    private float interpolationTime = 2;

    private void Start()
    {
        originalScale = transform.localScale;
        targetScale = Vector2.one * modifier;
        interpolateScale = true;
    }

    private void FixedUpdate()
    {
        if (interpolateScale)
        {
            currentTime += Time.deltaTime;
            InterpolateScale();
        }
    }

    private void OnDestroy()
    {
        transform.localScale = Vector2.one * modifier;
    }

    private void InterpolateScale() 
    {
        float ratio = currentTime / interpolationTime;
        Vector2 speed = Vector2.zero;

        transform.localScale = Vector2.Lerp(originalScale, targetScale, ratio);

        if (ratio >= 1)
            interpolateScale = false;
    }
}
