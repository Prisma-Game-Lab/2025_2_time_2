using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeEffect : CommandEffect
{
    private static SizeEffect activeInstance;

    private float scaleModifier;

    private Vector2 initialScale;
    private Vector2 targetScale;

    private bool interpolateScale;
    private float currentTime;
    private float interpolationTime = 2;

    private void Start()
    {
        if (activeInstance != null)
            StartCoroutine(activeInstance.DestructionSequence());
        activeInstance = this;

        initialScale = transform.localScale;
        targetScale = Vector2.one * modifier;
        interpolateScale = true;
    }

    private void FixedUpdate()
    {
        if (interpolateScale)
        {
            currentTime += Time.deltaTime;
            InterpolateScale(initialScale, targetScale);
        }
    }

    private void OnDestroy()
    {
        if (activeInstance == this)
            activeInstance = null;
    }

    private void InterpolateScale(Vector2 startScale, Vector2 targetScale) 
    {
        float ratio = currentTime / interpolationTime;

        transform.localScale = Vector2.Lerp(startScale, targetScale, ratio);

        if (ratio >= 1)
            interpolateScale = false;
    }

    private void ReturnToOriginalScale() 
    {
        currentTime = 0;
        targetScale = initialScale;
        initialScale = transform.localScale;
        interpolateScale = true;
    } 

    private IEnumerator DestructionSequence() 
    {
        ReturnToOriginalScale();
        yield return new WaitForSeconds(interpolationTime);
        Destroy(this);
    }
}
